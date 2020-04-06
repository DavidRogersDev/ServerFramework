using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Invariants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping
{
    public class Common
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static ApiResponse<BadRequest400Payload> ProcessInvalidModelState(ModelStateDictionary modelState)
        {
            var errors =
                modelState.ToDictionary(
                    k => k.Key, 
                    v => v.Value.Errors.Select(e => e.ErrorMessage)
                    );

            var payload = new ApiResponse<BadRequest400Payload>
            {
                Data = new BadRequest400Payload
                {
                    Errors = errors
                },
                Outcome = new OperationOutcome
                {
                    ErrorId = string.Empty,
                    Errors = errors.SelectMany(
                        keyValuePair => keyValuePair.Value
                            .Select(
                                str => string.Concat(keyValuePair.Key, GeneralPurpose.UniqueDelimiter, str)
                                )
                        ),
                    IsValidationFail = true,
                    IsError = false,
                    OpResult = OpResult.Fail
                }
            };

            return payload;
        }
    }
}
