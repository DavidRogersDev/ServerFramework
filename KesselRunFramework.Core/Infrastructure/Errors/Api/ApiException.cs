using System;

namespace KesselRunFramework.Core.Infrastructure.Errors.Api
{
    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {

        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public ApiException(string message, ExceptionGravity exceptionGravity = ExceptionGravity.Error, int statusCode = 500)
            : base(message)
        {
            ExceptionGravity = exceptionGravity;
            StatusCode = statusCode;
        }

        public ApiException(string message, Exception innerException, ExceptionGravity exceptionGravity = ExceptionGravity.Error, int statusCode = 500)
            : base(message, innerException)
        {
            ExceptionGravity = exceptionGravity;
            StatusCode = statusCode;
        }

        public ExceptionGravity ExceptionGravity { get; set; }
        public int StatusCode { get; set; }
    }
}
