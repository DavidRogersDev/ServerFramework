using System;

namespace KesselRunFramework.AspNet.Infrastructure.Services
{
    public interface ISessionStateService
    {
        bool? GetBoolean(string key);
        DateTime? GetDateTime(string key);
        DateTimeOffset? GetDateTimeOffset(string key);
        int? GetInt(string key);
        T GetObject<T>(string key);
        bool HasItem(string key);
        void SetBoolean(string key, bool value);
        void SetDateTime(string key, DateTime value);
        void SetDateTimeOffset(string key, DateTimeOffset value);

        void SetInt(string key, int value);
        void SetObject<T>(string key, T item);
    }
}
