using System;
using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Middleware
{
    public static class OptionsDelegates
    {
        public static void UpdateApiErrorResponse(HttpContext context, Exception ex, OperationOutcome operationOutcome)
        {
            if (ex.GetType().Name.Equals(typeof(SqlException).Name, StringComparison.OrdinalIgnoreCase))
            {
                operationOutcome.Message += "The exception was a database exception.";
            }
        }

        public static LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.OrdinalIgnoreCase))
            {
                return LogLevel.Critical;
            }
            return LogLevel.Error;
        }
    }
}
