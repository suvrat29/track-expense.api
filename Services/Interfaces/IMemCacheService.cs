namespace track_expense.api.Services.Interfaces
{
    public interface IMemCacheService
    {
        void SetValueInCache<T>(string username, string cacheKey, T value);
        T GetValueFromCache<T>(string username, string cacheKey);
        void RemoveValueFromCache(string username, string cacheKey);
        void ClearAllUserSpecificCache(string username);
        void UpdateValueInCache<T>(string username, string cacheKey, T value);
    }
}
