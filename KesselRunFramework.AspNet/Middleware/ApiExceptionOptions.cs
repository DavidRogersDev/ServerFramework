using System;
using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Middleware
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, OperationOutcome> AddResponseDetails { get; set; }
        public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
    }
}
