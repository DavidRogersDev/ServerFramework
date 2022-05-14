using System;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.Identity;
using KesselRunFramework.AspNet.Infrastructure.Services;
using KesselRunFramework.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            // For ASP.NET centric stuff, regester it with the framework container i.e. IServiceCollection
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<ISessionStateService, SessionStateService>();
            services.AddScoped<IRequestStateService, RequestStateService>();
            services.AddScoped(s => container.GetInstance<IDbResolver>());
            services.AddScoped<ICurrentUser, CurrentUserAdapter>();
            services.AddScoped<ApiExceptionFilter>();

            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() method wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    .AddControllerActivation();

                services.AddScoped<ILogger>(c => container.GetInstance<ILogger>());

                options.AddLogging(); // <-- This registers all logging abstractions      
                

                // Important for things like Http clients which are owned by the ServicesCollection but get
                // injected into things like MediatR request handlers, which use SimpleInjector for its IOC needs. 
                options.AutoCrossWireFrameworkComponents = true; 

                //.AddViewComponentActivation()
                //.AddPageModelActivation()
                //.AddTagHelperActivation();                
            });

        }
    }
}
