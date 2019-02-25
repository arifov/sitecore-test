using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;

namespace Sitecore.Membership.Cache
{
    public interface ICacheStorage
    {
        T GetOrCreate<T>(string cacheKey, Func<T> getValue, int cacheInterval = 60) where T : class;
        void Remove(string cacheKey);
    }

    public class MemoryCacheStorage : ICacheStorage
    {
        public T GetOrCreate<T>(string cacheKey, Func<T> getValue, int cacheInterval = 60) where T : class
        {
            var cache = MemoryCache.Default;

            if (!cache.Contains(cacheKey))
            {
                var cachedValue = getValue();
                if (cachedValue == null)
                    return null;

                cache.Add(cacheKey, cachedValue, DateTime.Now.AddMinutes(cacheInterval));
                return cachedValue;
            }

            return (T)cache[cacheKey];
        }

        public void Remove(string cacheKey)
        {
            var cache = MemoryCache.Default;
            cache.Remove(cacheKey);
        }
    }
}
