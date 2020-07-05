﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.ModelBinders
{
    /// <summary>
    /// Starting point for this class taken from this Github issue comment by Damien Edwards (member of the ASP.NET Core team):
    /// https://github.com/dotnet/aspnetcore/issues/11584#issuecomment-506007647
    /// </summary>
    public class UtcAwareDateTimeModelBinder : IModelBinder
    {
        private readonly DateTimeStyles _supportedStyles;
        private readonly ILogger _logger;

        public UtcAwareDateTimeModelBinder(DateTimeStyles supportedStyles, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _supportedStyles = supportedStyles;
            _logger = loggerFactory.CreateLogger<UtcAwareDateTimeModelBinder>();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                // no entry
                return Task.CompletedTask;
            }

            var modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            var metadata = bindingContext.ModelMetadata;
            var type = metadata.UnderlyingOrModelType;

            var value = valueProviderResult.FirstValue;
            var culture = valueProviderResult.Culture;

            object model;
            if (string.IsNullOrWhiteSpace(value))
            {
                model = null;
            }
            else if (type == typeof(DateTime))
            {
                // You could put custom logic here to sniff the raw value and call other DateTime.Parse overloads, e.g. forcing UTC
                model = DateTime.Parse(value, culture, _supportedStyles);
            }
            else
            {
                // unreachable
                throw new NotSupportedException();
            }

            // When converting value, a null model may indicate a failed conversion for an otherwise required
            // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
            // current bindingContext. If not, an error is logged.
            if (model == null && !metadata.IsReferenceOrNullableType)
            {
                modelState.TryAddModelError(
                    modelName,
                    metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                        valueProviderResult.ToString())
                );
            }
            else
            {
                _logger.TraceMessageModelBinderUsed(modelName, type.ToString(), nameof(UtcAwareDateTimeModelBinder));

                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }
}
