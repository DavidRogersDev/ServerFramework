using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Services
{
    public class RequestStateService : IRequestStateService
    {
        private readonly IDictionary<object, object> _requestItems;

        public RequestStateService(IHttpContextAccessor contextAccessor)
        {
            _requestItems = contextAccessor?.HttpContext?.Items;
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

        public void SetItem<T>(string key, T item)
        {
            _requestItems.Add(key, item);
        }
    }
}
