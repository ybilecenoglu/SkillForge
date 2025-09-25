using Castle.DynamicProxy;
using CoreTestFramework.Core.CrossCuttingConcern.Caching;

namespace CoreTestFramework.Core.Aspect.Autofac
{
    
    public class CacheRemoveInterceptorAspect : IInterceptor
    {
        private ICacheManager _cacheManager;

        public CacheRemoveInterceptorAspect(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Intercept(IInvocation invocation)
        {
            //Eğer metod üzerinde FluentValidationAttribute yoksa normal şekilde devam et
            var attribute = invocation.Method.GetCustomAttributes(true)
                        .OfType<CacheRemoveAttribute>()
                        .FirstOrDefault();

            // Eğer metod üzerinde ValidationAttribute yoksa
            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            _cacheManager.RemoveByPattern(attribute.Pattern);

             // Methodu çalıştır
            invocation.Proceed();
        }

    }
}