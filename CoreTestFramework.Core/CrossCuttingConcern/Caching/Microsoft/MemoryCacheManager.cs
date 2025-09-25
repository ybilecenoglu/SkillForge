using System.Reflection; //Cache içindeki özel alanlara (_coherentState, EntriesCollection) erişmek için.
using System.Text.RegularExpressions; // Regex bazlı cache temizliği için.
using Microsoft.Extensions.Caching.Memory; // Microsoft'un built-in bellek içi cache çözümü.

namespace CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager //ICache manager interface implement ediyor. İleride Redis Cache Manager yazarsam aynı interface'i implemente ederek sadece DI tarafında değiştirebiliriz.
    {
        private IMemoryCache MemoryCache;
        private readonly List<string> _cacheKeys = new List<string>(); //cache key taşıyacak listemiz
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache; //.Net Core DI container yönettiği IMemoryCache geliyor.
        }
        public void Add(string key, object data, int durationTime)
        {
            if (durationTime > 0)
                MemoryCache.Set(key, data, TimeSpan.FromMinutes(durationTime));
            else
                MemoryCache.Set(key, data);
                
            if (!_cacheKeys.Contains(key))
                _cacheKeys.Add(key);
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

        public void Remove(object key)
        {
            MemoryCache.Remove(key);
        }
        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var keysToRemove = _cacheKeys.Where(k => regex.IsMatch(k)).ToList();
            
            keysToRemove.ForEach(key =>
            {
                Remove(key);
                _cacheKeys.Remove(key);
            });
        }
    }
}