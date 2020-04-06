namespace KesselRunFramework.AspNet.Infrastructure.Invariants
{
    public sealed class Errors
    {
        public const string UnhandledErrorDebug = @"An unhandled error occurred. {0}";
        public const string UnhandledError = @"An error has occurred in the Web API.  " +
                                             "Please contact our support team if the problem persists, citing the reference Id of the Error Message. Reference Id: {0}";
        public const string ValidationFailure = @"Validation errors have occurred at the server.";
    }
}
