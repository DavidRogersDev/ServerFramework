using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KesselRunFramework.Core.Infrastructure.Errors.Api
{
    public class ApiValidationException : Exception
    {
        private readonly IList<string> _errorMessages;

        public ApiValidationException(string message, IList<string> validationErrors = null)
            : base(message)
        {
            _errorMessages = validationErrors ?? new List<string>();

        }

        public ApiValidationException(string message, Exception innerException, IList<string> validationErrors = null)
            : base(message, innerException)
        {
            _errorMessages = validationErrors ?? new List<string>();
        }

        public IReadOnlyCollection<string> Errors => new ReadOnlyCollection<string>(_errorMessages);
    }
}
