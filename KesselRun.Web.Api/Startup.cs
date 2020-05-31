using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Infrastructure.Ioc;
using KesselRun.Web.Api.Infrastructure.Mapping;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
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
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                    c =>
                    {
                        c.Filters.Add(typeof(SerilogMvcLoggingAttribute));
                        c.Filters.Add(typeof(ApiExceptionFilter));
                    })
                .ConfigureApiBehaviorOptions(ApiBehaviourConfigurer.ConfigureApiBehaviour)
                .AddJsonOptions(JsonOptionsConfigurer.ConfigureJsonOptions)
                .AddFluentValidation(fv => fv.ValidatorFactory = new SiteFluentValidatorFactory(Container));

            services.AddAppApiVersioning().AddSwagger(WebHostEnvironment, Configuration);
            services.ConfigureAppServices(WebHostEnvironment, Container);

            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle(); // This is default anyway

            var httpClientTypes = typeof(Startup).GetTypeInfo().Assembly.GetExportedTypes()
                    .Where(t => t.IsClass && typeof(ITypedHttpClient).IsAssignableFrom(t));

            services.RegisterTypedHttpClients(httpClientTypes);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjectorForDomain(Container);

            RegisterApplicationServices();

            app.ConfigureMiddlewareForEnvironments(env);

            app.UseApiExceptionHandler(opts =>
            {
                opts.AddResponseDetails = OptionsDelegates.UpdateApiErrorResponse;
                opts.DetermineLogLevel = OptionsDelegates.DetermineLogLevel;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = RequestLoggingConfigurer.EnrichFromRequest);

            app.UseSwaggerInDevAndStaging(WebHostEnvironment);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterApplicationServices()
        {
            var assemblies = GetAssemblies();

            Container.RegisterSingleton<ITypedClientResolver, TypedClientResolver>();

            Container.RegisterValidationAbstractions(new[] { assemblies[StartUp.Executing], assemblies[StartUp.Domain] });
            Container.RegisterAutomapperAbstractions(GetAutoMapperProfiles(assemblies));
            Container.RegisterMediatRAbstractions(new[] { assemblies[StartUp.Executing] }, GetTypesForPipeline(WebHostEnvironment));
            Container.RegisterApplicationServices(assemblies[StartUp.Domain], Configuration, "KesselRun.Business.ApplicationServices");
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
                {StartUp.Executing, typeof(Startup).GetTypeInfo().Assembly},
                {StartUp.Domain, typeof(RegisterUserPayloadDto).GetTypeInfo().Assembly }
            };

            // include any custom (domain) assemblies which will require scanning as part of the startup process.

            return assemblies;
        }

        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        private static Profile[] GetAutoMapperProfiles(IDictionary<string, Assembly> configurationAssemblies)
        {
            var abatementsApiProfile = new KesselRunApiProfile("KesselRunApiProfile");
            abatementsApiProfile.InitializeMappings(configurationAssemblies[StartUp.Domain].InArray());

            return abatementsApiProfile.InArray();
        }

    }
}
