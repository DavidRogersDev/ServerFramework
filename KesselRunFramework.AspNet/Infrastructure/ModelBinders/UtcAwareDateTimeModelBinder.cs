using System;
using System.Globalization;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.ModelBinders
{
    /// <summary>
    /// Starting point for this class taken from this Github issue comment by Damien Edwards (member of the ASP.NET Core team):
    /// https://github.com/dotnet/aspnetcore/issues/11584#issuecomment-506007647
    /// </summary>
    public class UtcAwareDateTimeModelBinder : IModelBinder
    {
        private readonly ILogger _logger;

        public UtcAwareDateTimeModelBinder(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<UtcAwareDateTimeModelBinder>();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            _logger.AttemptToBindModel(bindingContext);

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                _logger.ValueNotFoundInRequest(bindingContext);

                // no entry
                _logger.AttemptToBindModelDone(bindingContext);

                return Task.CompletedTask;
            }

            var modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            var metadata = bindingContext.ModelMetadata;
            var type = metadata.UnderlyingOrModelType;

            try
            {
                var value = valueProviderResult.FirstValue;

                object model;
                if (string.IsNullOrWhiteSpace(value))
                {
                    model = null;
                }
                else if (type == typeof(DateTime))
                {
                    if (value.EndsWith("Z"))
                    {
                        model = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AllowWhiteSpaces);
                    }
                    else
                    {
                        model = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces);
                    }
                }
                else
                {
                    // should be unreachable
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
                            valueProviderResult.ToString()
                            )
                    );
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            catch (Exception exception)
            {
                // Conversion failed.
                modelState.TryAddModelError(modelName, exception, metadata);
            }

            _logger.AttemptToBindModelDone(bindingContext);

            return Task.CompletedTask;
        }
    }
}
