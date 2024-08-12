using Microsoft.Extensions.Caching.Memory;
using NextechDemo.Shared.Models;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Application.Services.CacheService
{
    /// <summary>
    ///     Cache handling
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheOptions _cacheOptions;

        public CacheService(IMemoryCache memoryCache, CacheOptions cacheOptions)
        {
            _memoryCache = memoryCache;
            _cacheOptions = cacheOptions;
        }
       
        /// <summary>
        ///     Default cache key, its used for store top news ids
        /// </summary>
        /// <returns></returns>
        public string defaultCache() => "cachekey.topNews";
        /// <summary>
        ///     Get cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public T Get<T>(string key)
        {
            var result = default(T);

            try
            {
                _memoryCache.TryGetValue(key, out result);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return result;
        }
        /// <summary>
        ///     Set cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public T Set<T>(string key, T value, int absoluteExpiration = 0)
        {
            var result = default(T);
            try
            {
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = absoluteExpiration == 0 ? DateTime.Now.AddSeconds(_cacheOptions.AbsoluteExpiration) : DateTime.Now.AddSeconds(absoluteExpiration),
                    Priority = _cacheOptions.Priority,
                    SlidingExpiration = TimeSpan.FromSeconds(_cacheOptions.SlidingExpiration)
                };

                _memoryCache.Set(key, value, cacheExpiryOptions);
                result = value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return result;
        }
        /// <summary>
        ///     prepare cache key based on condition
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public string PrepareCacheKey(string key)
        {
            string result;
            try
            {
                result = string.Format(key);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return result;
        }
    }
}
