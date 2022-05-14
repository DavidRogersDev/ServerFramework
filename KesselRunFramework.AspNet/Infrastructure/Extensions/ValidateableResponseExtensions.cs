using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class ValidateableResponseExtensions
    {
        public static void AddToModelState(this ValidateableResponse source, ModelStateDictionary modelState)
        {
            foreach (var error in source.Errors)
            {
                foreach (var item in error.Value)
                {
                    modelState.AddModelError(error.Key, item);
                }
            }
        }

        public static IActionResult ToUnprocessableRequestResult(this ValidateableResponse source, string message = null)
        {
            var outcome = OperationOutcome.ValidationFailOutcome(source.Errors, message ?? string.Empty);

            var apiResponse = new ApiResponse<UnprocessableEntityPayload>
            {
                Data = null,
                Outcome = outcome
            };

            return new ObjectResult(apiResponse)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }
}
