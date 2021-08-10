using System;
using Microsoft.Extensions.Caching.Memory;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;

namespace track_expense.api.Services.ServiceClasses
{
    public class MemCacheService : IMemCacheService
    {
        #region Variables
        private readonly IApplogService _applogService;
        private readonly IMemoryCache _memCache;
        #endregion

        #region Constructor
        public MemCacheService(IApplogService applogService, IMemoryCache memCache)
        {
            _applogService = applogService;
            _memCache = memCache;
        }
        #endregion

        #region Public Methods
        public void SetValueInCache<T>(string username, string cacheKey, T value)
        {
            try
            {
                _memCache.Set(username + "$%$" + cacheKey, value);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "MemCacheService.cs", "SetValueInCache()");
                throw;
            }
        }

        public T GetValueFromCache<T>(string username, string cacheKey)
        {
            try
            {
                object cacheItem = null;
                bool itemExists = false;
                itemExists = _memCache.TryGetValue(username + "$%$" + cacheKey, out cacheItem);

                if (itemExists)
                    return (T)cacheItem;
                else
                    return default;
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "MemCacheService.cs", "GetValueFromCache()");
                throw;
            }
        }

        public void RemoveValueFromCache(string username, string cacheKey)
        {
            try
            {
                _memCache.Remove(username + "$%$" + cacheKey);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "MemCacheService.cs", "RemoveValueFromCache()");
                throw;
            }
        }

        public void ClearAllUserSpecificCache(string username)
        {
            try
            {
                _memCache.Remove(username + "$%$" + CacheKeyConstants.USER_LOGIN_CACHE_STORE);
                _memCache.Remove(username + "$%$" + CacheKeyConstants.USER_CACHE_STORE);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "MemCacheService.cs", "ClearAllUserSpecificCache()");
                throw;
            }
        }
        #endregion
    }
}
