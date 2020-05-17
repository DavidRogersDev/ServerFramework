using SimpleInjector;

namespace KesselRunFramework.Core.Infrastructure.Http
{
    public class TypedClientResolver : ITypedClientResolver
    {
        private readonly Container _container;

        public TypedClientResolver(Container container)
        {
            _container = container;
        }

        public T GetTypedClient<T>()
            where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
