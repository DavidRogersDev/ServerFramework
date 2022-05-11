using System;
using System.Collections.Generic;
using System.Linq;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Errors.Api;
using KesselRunFramework.Core.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace KesselRunFramework.AspNet.Infrastructure.ActionFilters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            // The operationOutcome is what we want to actually show the client.
            // Obviously, we do not want to display a Stacktrace or Exception details.
            // This does not deal with ModelState errors, as their is a separate ActionFilter for that.
            var operationOutcome = default(OperationOutcome);
            context.RouteData.Values.TryGetValue(Invariants.AspNet.Mvc.Controller, out var controller);
            context.RouteData.Values.TryGetValue(Invariants.AspNet.Mvc.Action, out var action);

            if (context.Exception is ApiValidationException apiValidationException)
            {
                operationOutcome = GetClientErrorPayload(Origin.ValidationError, apiValidationException);

                context.Exception = null;
                context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            }
            else
            {
                using (LogContext.PushProperty(Invariants.Logging.LogContexts.FromRoute,
                    string.Concat(controller?.ToString() ?? string.Empty, GeneralPurpose.UniqueDelimiter, action?.ToString() ?? string.Empty)))
                {
                    if (context.Exception is ApiException apiException)
                    {
                        // handle explicit 'known' API errors
                        context.Exception = null;

                        context.HttpContext.Response.StatusCode = apiException.StatusCode;

                        operationOutcome = GetClientErrorPayload(Origin.Action, apiException);

                        var level = apiException.ExceptionGravity == ExceptionGravity.Error
                            ? LogLevel.Error
                            : LogLevel.Critical;

                        _logger.Log(level,
                            EventIDs.EventIdAppThrown,
                            apiException,
                             MessageTemplates.DefaultLog,
                            apiException.Message,
                            operationOutcome.ErrorId
                            );
                    }
                    else
                    {
                        var exception = context.Exception;

                        // Unhandled errors
                        operationOutcome = GetClientErrorPayload(Origin.Unhandled, exception);

                        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        _logger.LogError(EventIDs.EventIdUncaught,
                            exception,
                            MessageTemplates.DefaultLog,
                            operationOutcome.Message,
                            operationOutcome.ErrorId
                            );

                        context.Exception = null;
                    }
                }
            }

            var wrappedPayload = new ApiResponse<IEnumerable<string>>
            {
                Data = null,
                Outcome = operationOutcome
            };

            // always return a JSON result
            context.Result = new JsonResult(wrappedPayload);

            base.OnException(context);
        }

        private OperationOutcome GetClientErrorPayload(Origin origin, Exception exception)
        {
            var operationOutcome = default(OperationOutcome);

            switch (origin)
            {
                case Origin.ValidationError:
                    operationOutcome = OperationOutcome.ValidationFailOutcome(
                        ((ApiValidationException)exception).Errors,
                        Errors.ValidationFailure
                        );
                    break;

                case Origin.Action:
                case Origin.Unhandled:

                    operationOutcome = OperationOutcome.UnSuccessfulOutcome;
                    operationOutcome.ErrorId = Guid.NewGuid().ToString();

#if DEBUG
                    operationOutcome.Message = string.Format(Errors.UnhandledErrorDebug, exception.GetBaseException().Message);
                    operationOutcome.Errors = exception.StackTrace.Split(
                        Environment.NewLine, StringSplitOptions.RemoveEmptyEntries
                        ).Select(str => str.Trim()); // ease of readability, for debugging purposes 👍
#else
                    operationOutcome.Message = string.Format(Errors.UnhandledError, operationOutcome.ErrorId);
#endif

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            return operationOutcome;
        }

        private enum Origin
        {
            Action,
            ValidationError,
            Unhandled
        }
    }
}
