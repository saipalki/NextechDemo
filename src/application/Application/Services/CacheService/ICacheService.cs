namespace NextechDemo.Application.Services.CacheService
{
    public interface ICacheService
    {
        public string defaultCache();
        T Get<T>(string key);
        T Set<T>(string key, T value, int absoluteExpiration = 0);
        public string PrepareCacheKey(string key);
    }
}
