using System.Linq;
using System.Text.Json;
using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping
{
    public class Common
    {
        public static  JsonSerializerOptions JsonSerializerOptions { get; set; }

        public static ApiResponse<BadRequest400Payload> ProcessInvalidModelState(ModelStateDictionary modelState)
        {
            var errors = GetErrorsFromModelState(modelState);

            var payload = new ApiResponse<BadRequest400Payload>
            {
                Data = null,
                Outcome = OperationOutcome.ValidationFailOutcome(errors)
            };

            return payload;
        }

        public static Dictionary<string, IEnumerable<string>> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState
                    .Where(m => m.Value.Errors.Any())
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage)
                    );

            return errors;
        }
    }
}
