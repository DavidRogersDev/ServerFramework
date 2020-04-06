using System.Collections.Generic;
using System.Linq;

namespace KesselRunFramework.AspNet.Response
{
    public class OperationOutcome
    {
        public OperationOutcome()
        {
            Message = string.Empty;
            ErrorId = string.Empty;
            Errors = Enumerable.Empty<string>();
        }

        public OpResult OpResult { get; set; }
        public string ErrorId { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public bool IsValidationFail { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static OperationOutcome SuccessfulOutcome => new OperationOutcome
        {
            Errors = Enumerable.Empty<string>(),
            ErrorId = string.Empty,
            IsError = false,
            IsValidationFail = false,
            Message = string.Empty,
            OpResult = OpResult.Success
        };

        public static OperationOutcome UnSuccessfulOutcome => new OperationOutcome
        {
            Errors = Enumerable.Empty<string>(),
            ErrorId = string.Empty,
            IsError = true,
            IsValidationFail = false,
            Message = string.Empty,
            OpResult = OpResult.Fail
        };
    }
}