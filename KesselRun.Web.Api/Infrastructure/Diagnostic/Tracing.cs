using System;
using KesselRun.Business.DataTransferObjects;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.Infrastructure.Diagnostic
{
    public static class Tracing
    {
        private static readonly Action<ILogger, RegisterUserPayloadDto, Exception> RegisteringUserTrace;

        const string PipelineHandler = "Pipeline Handler: ";

        static Tracing()
        {
            RegisteringUserTrace = LoggerMessage.Define<RegisterUserPayloadDto>(
                LogLevel.Debug,
                new EventId((int)WebTraceEventIdentifiers.RegisteringUserTrace, nameof(TraceRegisteringUser)), 
                PipelineHandler + "{@registerUserPayload}"
                );
        }

        public static void TraceRegisteringUser(this ILogger logger, RegisterUserPayloadDto registerUserPayload)
        {
            RegisteringUserTrace(logger, registerUserPayload, null);
        }

    }
}
