using System;
using System.Net;
using KesselRunFramework.AspNet.Infrastructure.Extensions;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InControl.Framework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class ServicesCollectionExtensions
    {
        public static void AddCorsByEnvironment(this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment,
            Action<CorsPolicyBuilder> buildAction = null)
        {
            if (webHostEnvironment.IsDevelopmentOrStaging())
            {
                services.AddCors(options =>
                    options.AddPolicy(Browser.CorsPolicy.AllowAll,
                        p => p.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                    )
                );
            }
            else
            {
                //  complete Cors config here for production
            }
        }

        public static void AddRedirect(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, double maxAge, int port = 443)
        {
            var isProduction = webHostEnvironment.IsProduction();

            if (isProduction)
            {
                // Only add Hsts for production.
                services.AddHsts(c =>
                {
                    c.Preload = false;
                    c.IncludeSubDomains = false;
                    c.MaxAge = TimeSpan.FromDays(maxAge);
                });
            }

            services.AddHttpsRedirection(c =>
            {
                c.HttpsPort = port;
                c.RedirectStatusCode = isProduction
                    ? (int)HttpStatusCode.PermanentRedirect
                    : (int)HttpStatusCode.TemporaryRedirect;
            });
        }
    }
}
