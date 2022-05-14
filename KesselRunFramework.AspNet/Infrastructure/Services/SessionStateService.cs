using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Services
{
    public class SessionStateService : ISessionStateService
    {
        private readonly ISession _sessionState;

        public SessionStateService(IHttpContextAccessor contextAccessor)
        {
#if DEBUG
            _sessionState = contextAccessor?.HttpContext?.Session; // is null when container.Verify() is called. Only called in Development.
#else
            _sessionState = contextAccessor.HttpContext.Session;
#endif
        }

        public bool? GetBoolean(string key)
        {
            var data = _sessionState.Get(key);

            if (ReferenceEquals(data, null)) return null;

            return BitConverter.ToBoolean(data, 0);
        }

        public DateTime? GetDateTime(string key)
        {
            var data = _sessionState.Get(key);

            if (ReferenceEquals(data, null)) return null;

            long ticks = BitConverter.ToInt64(data, 0);

            return new DateTime(ticks);
        }

        public DateTimeOffset? GetDateTimeOffset(string key)
        {
            var data = _sessionState.GetString(key);

            if (ReferenceEquals(data, null)) return null;

            return JsonSerializer.Deserialize<DateTimeOffset>(data);
        }

        public T GetObject<T>(string key)
        {
            var value = _sessionState.GetString(key);

            return value == null 
                ? default(T) 
                : JsonSerializer.Deserialize<T>(value);
        }

        public int? GetInt(string key)
        {
            return _sessionState.GetInt32(key);
        }

        public bool HasItem(string key)
        {
            return _sessionState.Keys.Contains(key);
        }

        public void SetBoolean(string key, bool value)
        {
            _sessionState.Set(key, BitConverter.GetBytes(value));
        }

        public void SetDateTime(string key, DateTime value)
        {
            long ticks = value.Ticks;
            var data = BitConverter.GetBytes(ticks);

            _sessionState.Set(key, data);
        }

        public void SetDateTimeOffset(string key, DateTimeOffset value)
        {
            _sessionState.SetString(key, JsonSerializer.Serialize(value));
        }

        public void SetObject<T>(string key, T item)
        {
            _sessionState.SetString(key, JsonSerializer.Serialize(item));
        }

        public void SetInt(string key, int value)
        {
            _sessionState.SetInt32(key, value);
        }
    }
}
