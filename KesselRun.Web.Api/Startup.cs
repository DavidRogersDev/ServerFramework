using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using InControl.Framework.AspNet.Infrastructure.Bootstrapping.Config;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Business.Validation;
using KesselRun.Web.Api.Infrastructure.Bootstrapping;
using KesselRun.Web.Api.Infrastructure.Mapping;
using KesselRun.Web.Api.New;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Messaging.Pipelines;
using KesselRunFramework.AspNet.Middleware;
using KesselRunFramework.AspNet.Validation;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using SimpleInjector;

namespace KesselRun.Web.Api
{
    public class Startup
    {
        private readonly Container Container = new Container();

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
            Assemblies = GetAssemblies();
        }

        public IConfiguration Configuration { get; }
        public IEnumerable<string> Versions { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public IEnumerable<Type> ExportedTypesWebAssembly { get; set; }
        IDictionary<string, Assembly> Assemblies { get; set; }
        public AppConfiguration AppConfiguration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(MvcConfigurer.ConfigureMvcOptions)
                .ConfigureApiBehaviorOptions(ApiBehaviourConfigurer.ConfigureApiBehaviour)
                .AddJsonOptions(JsonOptionsConfigurer.ConfigureJsonOptions)
                .AddFluentValidation(fv => 
                    fv.RegisterValidatorsFromAssemblyContaining<ColorCollectionValidator>(lifetime: ServiceLifetime.Singleton)
                    );

            AppConfiguration = StartupConfigurer.GetAppConfiguration(Configuration);

            Versions = AppConfiguration.GeneralConfig.OpenApiInfoList.Select(i => i.Version); // stash this for use in the Configure method below.
            services.AddAppApiVersioning().AddSwagger(WebHostEnvironment, Configuration, AppConfiguration.GeneralConfig.OpenApiInfoList);
            
            services.ConfigureAppServices(WebHostEnvironment, Container);
            
            ExportedTypesWebAssembly = Assemblies[StartUpConfig.Executing].GetExportedTypes();

            var httpClientTypes = ExportedTypesWebAssembly
                    .Where(t => t.IsClass && typeof(ITypedHttpClient).IsAssignableFrom(t));

            //services.AddRedirect(WebHostEnvironment, GeneralConfig.Hsts.MaxAge);

            services.RegisterTypedHttpClients(httpClientTypes);

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjectorForDomain(Container);

            RegisterApplicationServices();

            // commands and command decorators
            Container.Register(typeof(ICommandHandler<,>), Assemblies[StartUpConfig.Executing]);
            Container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(LogContextDecorator<,>));
            Container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(BusinessValidationDecorator<,>));

            // queries and quert decorators
            Container.Register(typeof(IQueryHandler<,>), Assemblies[StartUpConfig.Executing]);


            app.ConfigureMiddlewareForEnvironments(env, Container);

            app.UseApiExceptionHandler(opts =>
            {
                opts.AddResponseDetails = OptionsDelegates.UpdateApiErrorResponse;
                opts.DetermineLogLevel = OptionsDelegates.DetermineLogLevel;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = RequestLoggingConfigurer.EnrichFromRequest);

            app.UseSwaggerInDevAndStaging(WebHostEnvironment, Versions.ToArray());

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterApplicationServices()
        {
            Container.RegisterSingleton<ITypedClientResolver, TypedClientResolver>();

            Container.RegisterValidationAbstractions(new[] { Assemblies[StartUpConfig.Executing], Assemblies[StartUpConfig.Domain] });
            Container.RegisterAutomapperAbstractions(GetAutoMapperProfiles(Assemblies));
            Container.RegisterMediatRAbstractions(new[] { Assemblies[StartUpConfig.Executing] }, GetTypesForPipeline(WebHostEnvironment));
            Container.RegisterApplicationServices(Assemblies[StartUpConfig.Domain], Configuration, "KesselRun.Business.ApplicationServices");
        }

        private static Type[] GetTypesForPipeline(IWebHostEnvironment webHostEnvironment)
        {
            return webHostEnvironment.IsDevelopmentOrIsStaging()
                ? new[]
                {
                    typeof(OperationProfilingPipeline<,>), // not for Production
                    typeof(LogContextPipeline<,>),
                    typeof(BusinessValidationPipeline<,>)
                }
                : new[]
                {
                    typeof(LogContextPipeline<,>),
                    typeof(BusinessValidationPipeline<,>)
                };
        }

        private static IDictionary<string, Assembly> GetAssemblies()
        {
            var assemblies = new Dictionary<string, Assembly>(StringComparer.Ordinal)
            {
                {StartUpConfig.Executing, typeof(Startup).GetTypeInfo().Assembly},
                {StartUpConfig.Domain, typeof(RegisterUserPayloadDto).GetTypeInfo().Assembly }
            };

            // include any custom (domain) assemblies which will require scanning as part of the startup process.

            return assemblies;
        }
        
        private static Profile[] GetAutoMapperProfiles(IDictionary<string, Assembly> configurationAssemblies)
        {
            var kesselRunApiProfile = new KesselRunApiProfile("KesselRunApiProfile");
            kesselRunApiProfile.InitializeMappings(configurationAssemblies[StartUpConfig.Domain].InArray());

            return kesselRunApiProfile.InArray();
        }
    }
}
