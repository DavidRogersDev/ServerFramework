using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Services
{
    public class RequestStateService : IRequestStateService
    {
        private readonly IDictionary<object, object> _requestItems;

        public RequestStateService(IHttpContextAccessor contextAccessor)
        {
#if DEBUG
            _requestItems = contextAccessor?.HttpContext?.Items;
#else
            _requestItems = contextAccessor.HttpContext.Items;
#endif
        }

        public T GetItem<T>(string key)
        {
            if (_requestItems.TryGetValue(key, out var item))
                return (T) item;

            return default;
        }

        public bool HasItem(string key)
        {
            return _requestItems.ContainsKey(key);
        }

        public bool RemoveItem(string key)
        {
            return _requestItems.Remove(key);
        }

        public void SetItem<T>(string key, T item)
        {
            _requestItems.Add(key, item);
        }
    }
}
