using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp.Caching
{
    [PSerializable]//Bir türe uygulandığında PostSharp'ın PortableFormatter tarafından kullanılmak üzere bir serileştirici oluşturmasına neden olan özel öznitelik.
    public class CacheRemoveAspect : OnMethodBoundaryAspect
    {
        private ICacheManager _cacheManager;
        private string _pattern;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            _cacheManager.RemoveByPattern(_pattern);

        }
    }
}