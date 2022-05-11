using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.Logging
{
    public static class ModelBinderLoggingExtensions
    {
        private static readonly Action<ILogger, Type, string, Exception> AttemptingToBindModel;
        private static readonly Action<ILogger, string, Type, string, Exception> AttemptingToBindParameterModel;
        private static readonly Action<ILogger, Type, string, Type, string, Exception> AttemptingToBindPropertyModel;
        private static readonly Action<ILogger, Type, string, Exception> DoneAttemptingToBindModel;
        private static readonly Action<ILogger, string, Type, Exception> DoneAttemptingToBindParameterModel;
        private static readonly Action<ILogger, Type, string, Type, Exception> DoneAttemptingToBindPropertyModel;
        private static readonly Action<ILogger, string, Type, Exception> FoundNoValueInRequest;
        private static readonly Action<ILogger, string, string, Type, Exception> FoundNoValueForParameterInRequest;
        private static readonly Action<ILogger, string, Type, string, Type, Exception> FoundNoValueForPropertyInRequest;

        static ModelBinderLoggingExtensions()
        {
            AttemptingToBindModel = LoggerMessage.Define<Type, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindModelTrace, nameof(AttemptToBindModel)),
                "Attempting to bind model of type '{ModelType}' using the name '{ModelName}' in request data ..."
                );

            AttemptingToBindParameterModel = LoggerMessage.Define<string, Type, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindParameterModelTrace, nameof(AttemptToBindModel)),
                "Attempting to bind parameter {ParameterName} of type {ModelType} using the name {ModelName} in request data ..."
                );

            AttemptingToBindPropertyModel = LoggerMessage.Define<Type, string, Type, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindPropertyModelTrace, nameof(AttemptToBindModel)),
                "Attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}' using the name '{ModelName}' in request data ..."
                );

            DoneAttemptingToBindModel = LoggerMessage.Define<Type, string>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindModelTrace, nameof(AttemptToBindModel)),
                "Done attempting to bind model of type '{ModelType}' using the name '{ModelName}'."
                );

            DoneAttemptingToBindParameterModel = LoggerMessage.Define<string, Type>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindParameterModelTrace, nameof(AttemptToBindModel)),
                "Done attempting to bind parameter '{ParameterName}' of type '{ModelType}'."
                );

            DoneAttemptingToBindPropertyModel = LoggerMessage.Define<Type, string, Type>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.AttemptingToBindPropertyModelTrace, nameof(AttemptToBindModel)),
                "Done attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}'."
                );

            FoundNoValueInRequest = LoggerMessage.Define<string, Type>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.FoundNoValueInRequestTrace, nameof(ValueNotFoundInRequest)),
                "Could not find a value in the request with name '{ModelName}' of type '{ModelType}'."
                );

            FoundNoValueForParameterInRequest = LoggerMessage.Define<string, string, Type>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.FoundNoValueForParameterInRequestTrace, nameof(ValueNotFoundInRequest)),
                "Could not find a value in the request with name '{ModelName}' for binding parameter '{ModelFieldName}' of type '{ModelType}'."
                );

            FoundNoValueForPropertyInRequest = LoggerMessage.Define<string, Type, string, Type>(
                LogLevel.Debug,
                new EventId((int)TraceEventIdentifiers.FoundNoValueForPropertyInRequestTrace, nameof(ValueNotFoundInRequest)),
                "Could not find a value in the request with name '{ModelName}' for binding property '{PropertyContainerType}.{ModelFieldName}' of type '{ModelType}'."
                );
        }

        public static void AttemptToBindModel(this ILogger logger, ModelBindingContext bindingContext)
        {
            var modelMetadata = bindingContext.ModelMetadata;
            switch (modelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Parameter:
                    AttemptingToBindParameterModel(
                        logger,
                        modelMetadata.ParameterName,
                        modelMetadata.ModelType,
                        bindingContext.ModelName,
                        null
                        );
                    break;
                case ModelMetadataKind.Property:
                    AttemptingToBindPropertyModel(
                        logger,
                        modelMetadata.ContainerType,
                        modelMetadata.PropertyName,
                        modelMetadata.ModelType,
                        bindingContext.ModelName,
                        null
                        );
                    break;
                case ModelMetadataKind.Type:
                    AttemptingToBindModel(logger, bindingContext.ModelType, bindingContext.ModelName, null);
                    break;
            }
        }

        public static void AttemptToBindModelDone(this ILogger logger, ModelBindingContext bindingContext)
        {
            var modelMetadata = bindingContext.ModelMetadata;

            switch (modelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Parameter:
                    DoneAttemptingToBindParameterModel(
                        logger,
                        modelMetadata.ParameterName,
                        modelMetadata.ModelType,
                        null
                        );
                    break;
                case ModelMetadataKind.Property:
                    DoneAttemptingToBindPropertyModel(
                        logger,
                        modelMetadata.ContainerType,
                        modelMetadata.PropertyName,
                        modelMetadata.ModelType,
                        null
                        );
                    break;
                case ModelMetadataKind.Type:
                    DoneAttemptingToBindModel(
                        logger, 
                        bindingContext.ModelType, 
                        bindingContext.ModelName, 
                        null
                        );
                    break;
            }
        }

        public static void ValueNotFoundInRequest(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            var modelMetadata = bindingContext.ModelMetadata;
            switch (modelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Parameter:
                    FoundNoValueForParameterInRequest(
                        logger,
                        bindingContext.ModelName,
                        modelMetadata.ParameterName,
                        bindingContext.ModelType,
                        null
                        );
                    break;
                case ModelMetadataKind.Property:
                    FoundNoValueForPropertyInRequest(
                        logger,
                        bindingContext.ModelName,
                        modelMetadata.ContainerType,
                        modelMetadata.PropertyName,
                        bindingContext.ModelType,
                        null
                        );
                    break;
                case ModelMetadataKind.Type:
                    FoundNoValueInRequest(
                        logger,
                        bindingContext.ModelName,
                        bindingContext.ModelType,
                        null
                        );
                    break;
            }
        }
    }
}
