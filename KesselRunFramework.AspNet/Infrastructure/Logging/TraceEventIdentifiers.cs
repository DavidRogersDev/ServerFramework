namespace KesselRunFramework.AspNet.Infrastructure.Logging
{
    public enum TraceEventIdentifiers
    {
        ProfileMessagingTrace = 50,
        BeforeValidatingMessageTrace = 51,
        InValidMessageTrace = 52,
        ValidMessageTrace = 53,
        ModelBinderUsedTrace = 54,
        AttemptingToBindModelTrace = 55,
        AttemptingToBindParameterModelTrace = 56,
        AttemptingToBindPropertyModelTrace = 57,
        FoundNoValueInRequestTrace = 58,
        FoundNoValueForParameterInRequestTrace = 59,
        FoundNoValueForPropertyInRequestTrace = 60,
        DoneAttemptingToBindModelTrace = 61,
        DoneAttemptingToBindParameterModelTrace = 62,
        DoneAttemptingToBindPropertyModelTrace = 63
    }
}