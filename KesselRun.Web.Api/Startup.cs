using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Infrastructure.Ioc;
using KesselRun.Web.Api.Infrastructure.Mapping;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Messaging.Pipelines;
using KesselRunFramework.AspNet.Middleware;
using KesselRunFramework.AspNet.Validation;
using KesselRunFramework.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public IConfiguration Configuration { get; }
        public IEnumerable<string> Versions { get; set; }
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

            var openApiInfos = GetOpenApiInfo("swaggerconfig.json");
            Versions = openApiInfos.Select(i => i.Version); // stash this for use in the Configure method below.
            services.AddAppApiVersioning().AddSwagger(WebHostEnvironment, Configuration, openApiInfos);
            services.ConfigureAppServices(WebHostEnvironment, Container);
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

            app.UseSwaggerInDevAndStaging(WebHostEnvironment, Versions.ToArray());

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
        
        private static Profile[] GetAutoMapperProfiles(IDictionary<string, Assembly> configurationAssemblies)
        {
            var kesselRunApiProfile = new KesselRunApiProfile("KesselRunApiProfile");
            kesselRunApiProfile.InitializeMappings(configurationAssemblies[StartUp.Domain].InArray());

            return kesselRunApiProfile.InArray();
        }

        private static IList<OpenApiInfo> GetOpenApiInfo(string swaggerSettingsFile)
        {
            var swaggerConfiguration = new ConfigurationBuilder()
                .SetBasePath(Program.BasePath)
                .AddJsonFile(swaggerSettingsFile)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<List<OpenApiInfo>>(p => p.GetRequiredService<IOptions<List<OpenApiInfo>>>().Value);
            services.Configure<List<OpenApiInfo>>(options => swaggerConfiguration.GetSection(nameof(OpenApiInfo)).Bind(options));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<List<OpenApiInfo>>();
        }
    }
}
