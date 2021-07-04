using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class MvcConfigurer
    {
        public static void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(typeof(SerilogMvcLoggingAttribute));
            mvcOptions.Filters.Add(typeof(ApiExceptionFilter));
        }
    }
}
