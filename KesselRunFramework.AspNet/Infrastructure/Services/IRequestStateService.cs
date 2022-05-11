﻿namespace KesselRunFramework.AspNet.Infrastructure.Services
{
    public interface IRequestStateService
    {
        T GetItem<T>(string key);
        bool HasItem(string key);
        void SetItem<T>(string key, T item);
    }
}
