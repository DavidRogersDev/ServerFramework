namespace KesselRunFramework.Core.Infrastructure.Logging
{
    public class MessageTemplates
    {
        public const string DefaultLog = "{ErrorMessage} -- {ErrorId}.";
        public const string UncaughtGlobal = "Uncaught Exception. {ResolvedExceptionMessage} -- {ErrorId}.";
        public const string ValidationErrorsLog = "Validation Fail. {Errors}. User Id {UserName}.";
    }
}
