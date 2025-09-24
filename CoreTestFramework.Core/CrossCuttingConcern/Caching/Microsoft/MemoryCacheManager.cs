using System.Reflection; //Cache içindeki özel alanlara (_coherentState, EntriesCollection) erişmek için.
using System.Text.RegularExpressions; // Regex bazlı cache temizliği için.
using Microsoft.Extensions.Caching.Memory; // Microsoft'un built-in bellek içi cache çözümü.

namespace CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager //ICache manager interface implement ediyor. İleride Redis Cache Manager yazarsam aynı interface'i implemente ederek sadece DI tarafında değiştirebiliriz.
    {
        private IMemoryCache MemoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache; //.Net Core DI container yönettiği IMemoryCache geliyor.
        }
        public void Add(string key, object data, int durationTime)
        {
            MemoryCache.Set(key, data, TimeSpan.FromMinutes(durationTime));
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
            var field = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance); //MemoryCache’in private field bilgisi alınıyoruz.

            var choerentStateValue = field.GetValue(MemoryCache); //MemoryCache nesnesi üzerinden bu field’ın değerini alıyoruz.

            var entriesCollection = choerentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance); //Private instance property’si olduğu için Reflection ile ulaşıyoruz. Bu, cache’de tutulan tüm entry’lerin saklandığı koleksiyon.
            
            var entriesCollectionValue = entriesCollection.GetValue(choerentStateValue) as dynamic; //entriesCollection.GetValue(choerentStateValue) → choerentStateValue (yani _coherentState) üzerinden EntriesCollection property’sinin değerini alıyoruz.as dynamic → Tipi normalde internal/private olduğu için C# derleyicisi bilmez. dynamic diyerek “runtime’da çöz” diyoruz.


            /*
            Burada Reflection + Regex kullanılarak cache içindeki tüm key’ler taranıyor:

            _coherentState → MemoryCache’in private alanı.

            EntriesCollection → İçindeki tüm cache kayıtları.

            Hepsi listeye alınıyor.

            Regex ile eşleşenler bulunuyor.

            Remove(key) ile tek tek siliniyor.
            */
            var cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in entriesCollectionValue)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            keysToRemove.ForEach(key =>
            {
                Remove(key);
            });
        }
    }
}