using System;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class ConfigurationAddExtensions
    {
        public static void ConfigureAppServices(this IServiceCollection services, IWebHostEnvironment env, Container container)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (env == null) throw new ArgumentNullException(nameof(env));
            if (container == null) throw new ArgumentNullException(nameof(container));

            // For ASP.NET centric stuff, regester it with the framework container i.e. IServiceCollection
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient(); // this registers IHttpClientFactory, which we inject into some services to get HttpClients.

            services.AddScoped<ICurrentUser, CurrentUserAdapter>();
            services.AddScoped<ApiExceptionFilter>();

            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle(); // This is default anyway

            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() method wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    .AddControllerActivation();

                options.AddLogging(); // <-- This registers all logging abstractions                

                //.AddViewComponentActivation()
                //.AddPageModelActivation()
                //.AddTagHelperActivation();                
            });
        }
    }
}
