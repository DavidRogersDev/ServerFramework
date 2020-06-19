using Microsoft.Extensions.Logging;

namespace KesselRunFramework.Core.Infrastructure.Errors
{
    public static class EventIDs
    {
        public static readonly EventId EventIdAppThrown = new EventId((int)EventIdIdentifier.AppThrown, EventIdIdentifier.AppThrown.ToString());
        public static readonly EventId EventIdHttpClient = new EventId((int)EventIdIdentifier.HttpClient, EventIdIdentifier.HttpClient.ToString());
        public static readonly EventId EventIdPipelineThrown = new EventId((int)EventIdIdentifier.PipelineThrown, EventIdIdentifier.PipelineThrown.ToString());
        public static readonly EventId EventIdUncaught = new EventId((int)EventIdIdentifier.UncaughtInAction, EventIdIdentifier.UncaughtInAction.ToString());
        public static readonly EventId EventIdUncaughtGlobal = new EventId((int)EventIdIdentifier.UncaughtGlobal, EventIdIdentifier.UncaughtGlobal.ToString());
    }
}
