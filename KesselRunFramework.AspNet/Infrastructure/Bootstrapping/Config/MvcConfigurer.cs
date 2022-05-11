using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.ModelBinders.Providers;
using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class MvcConfigurer
    {
        public static void ConfigureMvcOptions(MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(typeof(SerilogMvcLoggingAttribute));
            mvcOptions.Filters.Add(typeof(ApiExceptionFilter));
            mvcOptions.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
        }
    }
}
