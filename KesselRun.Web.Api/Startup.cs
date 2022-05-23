using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation.AspNetCore;
using InControl.Framework.AspNet.Infrastructure.Bootstrapping.Config;
using KesselRun.Web.Api.Infrastructure.Bootstrapping;
using KesselRun.Web.Api.Infrastructure.Ioc;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Ioc;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Middleware;
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
            Assemblies = StartupConfigurer.GetAssemblies();
        }

        public IConfiguration Configuration { get; set; }
        public IEnumerable<string> Versions { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        IDictionary<string, Assembly> Assemblies { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(MvcConfigurer.ConfigureMvcOptions)
                .ConfigureApiBehaviorOptions(ApiBehaviourConfigurer.ConfigureApiBehaviour)
                .AddJsonOptions(JsonOptionsConfigurer.ConfigureJsonOptions)
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblies(new[] { Assemblies[StartUpConfig.Domain], Assemblies[StartUpConfig.Executing] }, lifetime: ServiceLifetime.Singleton)
                    );

            var appConfiguration = StartupConfigurer.GetAppConfiguration(Configuration);

            Versions = appConfiguration.GeneralConfig.OpenApiInfoList.Select(i => i.Version); // stash this for use in the Configure method below.

            services.AddAppApiVersioning().AddSwagger(WebHostEnvironment, Configuration, appConfiguration.GeneralConfig.OpenApiInfoList);
            services.AddRedirect(WebHostEnvironment, appConfiguration.HstsSettings.MaxAge);
            services.ConfigureAppServices(WebHostEnvironment, Container);

            var httpClientTypes = Assemblies[StartUpConfig.Executing].GetExportedTypes()
                    .Where(t => t.IsClass && typeof(ITypedHttpClient).IsAssignableFrom(t));

            services.RegisterTypedHttpClients(httpClientTypes);

            WebHostEnvironment = null; // not needed any longer. GC can cleanup.
            Configuration = null; // not needed any longer. GC can cleanup.
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseSimpleInjectorForDomain(Container);

            Container.RegisterApplicationServices(configuration, Assemblies);

            Container.RegisterAspectParts(Assemblies);


            app.ConfigureMiddlewareForEnvironments(env, Container);

            app.UseApiExceptionHandler(opts =>
            {
                opts.AddResponseDetails = OptionsDelegates.UpdateApiErrorResponse;
                opts.DetermineLogLevel = OptionsDelegates.DetermineLogLevel;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = RequestLoggingConfigurer.EnrichFromRequest);

            app.UseSwaggerInDevAndStaging(env, Versions.ToArray());

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            Versions = null; // not needed any longer. GC can cleanup.
            Assemblies = null; // not needed any longer. GC can cleanup.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
