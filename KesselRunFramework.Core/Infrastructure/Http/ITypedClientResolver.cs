namespace KesselRunFramework.Core.Infrastructure.Http
{
    public interface ITypedClientResolver
    {
        T GetTypedClient<T>() where T : class;
    }
}