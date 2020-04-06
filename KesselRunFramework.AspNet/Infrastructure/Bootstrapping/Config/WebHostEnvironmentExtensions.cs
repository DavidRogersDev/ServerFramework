using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsDevelopmentOrIsStaging(this IWebHostEnvironment webHostEnvironment)
        {
            return webHostEnvironment.IsDevelopment() || webHostEnvironment.IsStaging();
        }
    }
}
