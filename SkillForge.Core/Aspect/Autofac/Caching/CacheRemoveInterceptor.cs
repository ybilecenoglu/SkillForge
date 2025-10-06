using Castle.DynamicProxy;
using SkillForge.Core.CrossCuttingConcern.Caching;

namespace SkillForge.Core.Aspect.Autofac
{
    
    public class CacheRemoveInterceptor : IInterceptor
    {
        private ICacheManager _cacheManager;

        public CacheRemoveInterceptor(ICacheManager cacheManager)
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