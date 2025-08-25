using Microsoft.Extensions.Caching.Memory;

namespace PersonalRegister.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void AddToCache(string key, object value)
        {
            _memoryCache.Set(key, value);
        }
        public object GetFromCache(string key)
        {
            if (_memoryCache.TryGetValue(key, out object value))
            {
                return value;
            }

            return null;
        }
        public void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
