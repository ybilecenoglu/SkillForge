using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp.Caching
{
    [PSerializable] //PostSharp’ın aspect’i serialize edebilmesi için gerekli. Aspect’ler compile-time’da IL koduna dokunduğu için bu zorunlu.
    public class CacheAspect : MethodInterceptionAspect //AOP Altyapısı. MethodInterceptionAspect ile methot çağrılarını kesiliyor. 
    {
        private int _durationTime;
        private ICacheManager _cacheManager; //Cross-Cutting Caching katmanındaki cache yöneticin.

        public CacheAspect(int durationTime=60)
        {
            _durationTime = durationTime;
            //_cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }
        public override void OnInvoke(MethodInterceptionArgs args) // Metot çağrılarında devreye giren override edilmiş method.
        {
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            var methodName = $"{args.Method.ReflectedType.FullName}.{args.Method.Name}"; // Çağrılan methodun adı.
            var arguments = args.Arguments.ToList(); //Parametreleri listeye çevirdik.
            var key =$"{methodName}({string.Join(",", arguments.Select(x => x?.ToString()??"<Null>"))})"; //Cache benzersiz anahtarı üretiliyor. Eğer parametre null ise null veriyoruz böylece key çakışmasını engelliyoruz.

            if (_cacheManager.IsAdd(key)) // cache daha önce eklenmişse methoda girmiyoruz.
            {
                args.ReturnValue = _cacheManager.Get(key);
                return;
            }
            args.Proceed(); // Çağıralan methodu çalıştıştır.
            _cacheManager.Add(key,args.ReturnValue, _durationTime); // Dönen sonucu cache ekle, böylece aynı parametre ile çağrılan metot yerine cache çalışmış olacak.

        }
    }
}