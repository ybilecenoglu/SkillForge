using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        private IMemoryCache MemoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }
        public void Add(string key, object data, int durationTime)
        {
           MemoryCache.Set(key,data,TimeSpan.FromMinutes(durationTime));
        }

        public T Get<T>(string key)
        {
            return MemoryCache.Get<T>(key); //Cache için aldığımız key veriyoruz.
        }

        public object Get(string key)
        {
            return MemoryCache.Get(key); //Anonim bir tip ise kullanacağımız method.
        }

        public bool IsAdd(string key)
        {
           return MemoryCache.TryGetValue(key, out _); //Cache var mı yok mu kontrol et
        }

        public void Remove(string key)
        {
            MemoryCache.Remove(key);
        }
    }
}