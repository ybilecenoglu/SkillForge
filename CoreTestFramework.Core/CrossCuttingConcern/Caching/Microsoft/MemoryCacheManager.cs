using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

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

        public void Remove(object key)
        {
            MemoryCache.Remove(key);
        }
        public void RemoveByPattern(string pattern)
        {
           var field = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
           var choerentStateValue = field.GetValue(MemoryCache);
           var entriesCollection = choerentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
           var entriesCollectionValue = entriesCollection.GetValue(choerentStateValue) as dynamic;

           var cacheCollectionValues = new List<ICacheEntry>();

           foreach (var cacheItem in entriesCollectionValue)
           {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem,null);
                cacheCollectionValues.Add(cacheItemValue);
           }

           var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
           var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

           keysToRemove.ForEach(key => {
            Remove(key);
           });
        }
    }
}