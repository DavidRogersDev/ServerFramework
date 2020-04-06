using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsDevelopmentOrStaging(this IWebHostEnvironment env)
        {
            return env.IsDevelopment() || env.IsStaging();
        }
    }
}
