using KesselRunFramework.Core.Infrastructure.Extensions;
using System.Collections.Generic;

namespace KesselRunFramework.Core.Infrastructure.Messaging
{
    public class ValidateableResponse
    {
        public ValidateableResponse(IReadOnlyDictionary<string, IEnumerable<string>> validationErrors = null)
        {
            Errors = validationErrors ?? new Dictionary<string, IEnumerable<string>>();
        }

        public bool IsValidResponse => Errors.None();

        public IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; }
    }

    public class ValidateableResponse<TModel> : ValidateableResponse
        where TModel : class
    {

        public ValidateableResponse(TModel model, IReadOnlyDictionary<string, IEnumerable<string>> validationErrors = null)
            : base(validationErrors)
        {
            Result = model;
        }

        public TModel Result { get; }
    }
}
