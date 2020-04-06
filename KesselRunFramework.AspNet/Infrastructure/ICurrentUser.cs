namespace KesselRunFramework.AspNet.Infrastructure
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
    }
}
