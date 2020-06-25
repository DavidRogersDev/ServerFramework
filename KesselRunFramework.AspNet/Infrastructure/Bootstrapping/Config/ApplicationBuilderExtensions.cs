using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureMiddlewareForEnvironments(this IApplicationBuilder app, 
            IWebHostEnvironment env,
            Container container)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                container.Verify();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }

        public static void UseSimpleInjectorForDomain(this IApplicationBuilder app, Container container)
        {
            // UseSimpleInjector() enables framework services to be injected into
            // application components, resolved by Simple Injector.
            app.UseSimpleInjector(container, options =>
            {
                // Add custom Simple Injector-created middleware to the ASP.NET pipeline.
                //options.UseMiddleware<CustomMiddleware1>(app);
                //options.UseMiddleware<CustomMiddleware2>(app);

                // Optionally, allow application components to depend on the
                // non-generic Microsoft.Extensions.Logging.ILogger
                // or Microsoft.Extensions.Localization.IStringLocalizer abstractions.
                
                //options.UseLocalization();
            });
        }
    }
}
