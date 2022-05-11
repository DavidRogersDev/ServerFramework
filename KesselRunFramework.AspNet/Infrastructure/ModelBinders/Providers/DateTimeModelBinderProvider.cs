using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.ModelBinders.Providers
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var modelType = context.Metadata.UnderlyingOrModelType;
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

            if (modelType == typeof(DateTime))
            {
                return new UtcAwareDateTimeModelBinder(loggerFactory);
            }

            return null;
        }
    }
}