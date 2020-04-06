using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly ApiExceptionOptions _options;

        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next,
            ILogger<ApiExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _options);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ApiExceptionOptions apiExceptionOptions)
        {
            var errorId = Guid.NewGuid().ToString();

            // This is what we tell the client.
            var outcome = new OperationOutcome
            {
                ErrorId = errorId,
                Errors = Enumerable.Empty<string>(),
                IsError = true,
                Message = string.Format(Errors.UnhandledError, errorId),
                OpResult = OpResult.Fail
            };

            apiExceptionOptions.AddResponseDetails?.Invoke(context, exception, outcome);

            // This is what we log.
            var resolvedExceptionMessage = GetInnermostExceptionMessage(exception);

            var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;
            
            _logger.Log(level,
                EventIDs.EventIdUncaughtGlobal,
                exception, 
                 MessageTemplates.UncaughtGlobal, 
                resolvedExceptionMessage, 
                errorId
                );

            var apiResponse = new ApiResponse<IEnumerable<string>>
            {
                Data = Enumerable.Empty<string>(), Outcome = outcome
            };

            var result = JsonSerializer.Serialize(apiResponse, Common.JsonSerializerOptions);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(result);
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (ReferenceEquals(exception.InnerException, null))
                return exception.Message;
            
            return GetInnermostExceptionMessage(exception.InnerException);
        }
    }
}
