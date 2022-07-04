using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        private static readonly Action<ILogger, Exception> BeforeValidatingMessageTrace;
        private static readonly Action<ILogger, IReadOnlyDictionary<string, IReadOnlyList<string>>, string, Exception> InvalidMessageTrace;
        private static readonly Action<ILogger, string, string, string, Exception> ModelBinderUsed;
        private static readonly Action<ILogger, long, Exception> ProfileMessagingTrace;
        private static readonly Action<ILogger, Exception> ValidMessageTrace;


        const string PipelineBehaviour = "Pipeline Behaviour: ";

        static LoggingExtensions()
        {
            BeforeValidatingMessageTrace = LoggerMessage.Define(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.BeforeValidatingMessageTrace, nameof(TraceBeforeValidatingMessage)),
                PipelineBehaviour + " Validating Message."
            );

            ProfileMessagingTrace = LoggerMessage.Define<long>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.ProfileMessagingTrace, nameof(TraceMessageProfiling)),
                PipelineBehaviour + " Request took {milliseconds} milliseconds."
                );

            InvalidMessageTrace = LoggerMessage.Define<IReadOnlyDictionary<string, IReadOnlyList<string>>, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.InValidMessageTrace, nameof(TraceMessageValidationFailed)),
                PipelineBehaviour + " Invalid Message. {errors}. User: {user}"
                );

            ModelBinderUsed = LoggerMessage.Define<string, string, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.ModelBinderUsedTrace, nameof(TraceMessageModelBinderUsed)),
                "Parameter '{modelName}' of type '{type}' bound using ModelBinder \"{modelBinder}\"."
            );

            ValidMessageTrace = LoggerMessage.Define(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.ValidMessageTrace, nameof(TraceMessageValidationPassed)),
                PipelineBehaviour + " Message is valid."
                );
        }

        public static void TraceMessageProfiling(this ILogger logger, long milliseconds)
        {
            ProfileMessagingTrace(logger, milliseconds, null);
        }

        public static void TraceMessageValidationFailed(this ILogger logger, IReadOnlyDictionary<string, IReadOnlyList<string>> errors, string user)
        {
            InvalidMessageTrace(logger, errors, user, null);
        }

        public static void TraceBeforeValidatingMessage(this ILogger logger)
        {
            BeforeValidatingMessageTrace(logger, null);
        }

        public static void TraceMessageModelBinderUsed(this ILogger logger, string parameter, string type, string modelBinder)
        {
            ModelBinderUsed(logger, parameter, type, modelBinder, null);
        }

        public static void TraceMessageValidationPassed(this ILogger logger)
        {
            ValidMessageTrace(logger, null);
        }
    }
}
