using System.Collections.Generic;
using System.Collections.ObjectModel;
using KesselRunFramework.Core.Infrastructure.Extensions;

namespace KesselRunFramework.Core.Infrastructure.Messaging
{
    public class ValidateableResponse
    {
        private readonly IDictionary<string, IEnumerable<string>> _errors;

        public ValidateableResponse(IDictionary<string, IEnumerable<string>> validationErrors = null)
        {
            _errors = validationErrors ?? new Dictionary<string, IEnumerable<string>>();
        }

        public bool IsValidResponse => _errors.None();

        public IReadOnlyDictionary<string, IEnumerable<string>> Errors => new ReadOnlyDictionary<string, IEnumerable<string>>(_errors);
    }

    public class ValidateableResponse<TModel> : ValidateableResponse
        where TModel : class
    {

        public ValidateableResponse(TModel model, IDictionary<string, IEnumerable<string>> validationErrors = null)
            : base(validationErrors)
        {
            Result = model;
        }

        public TModel Result { get; }
    }
}
