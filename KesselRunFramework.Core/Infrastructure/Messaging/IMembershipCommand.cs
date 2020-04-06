namespace KesselRunFramework.Core.Infrastructure.Messaging
{
    public interface IMembershipCommand
    {
        // marker interface. No members, by design.
        // This type of command is for operations which will use a DbContext specific to user management.
    }
}