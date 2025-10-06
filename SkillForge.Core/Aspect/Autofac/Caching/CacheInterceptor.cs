using System.Linq.Expressions;
using Castle.DynamicProxy;
using SkillForge.Core.CrossCuttingConcern.Caching;
using SkillForge.Core.Utilities; // Autofac DynamicProxy için gerekli namespace


namespace SkillForge.Core.Aspect.Autofac
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheManager _cacheManager;
        private readonly int _durationTime;

        // DI ile ICacheManager inject ediyoruz
        public CacheInterceptor(ICacheManager cacheManager, int durationTime = 60)
        {
            _cacheManager = cacheManager;
            _durationTime = durationTime;
        }
        public void Intercept(IInvocation invocation)
        {
            string key = string.Empty;
            //Eğer metod üzerinde FluentValidationAttribute yoksa normal şekilde devam et
            var attribute = invocation.Method.GetCustomAttributes(true)
                        .OfType<CacheAttribute>()
                        .FirstOrDefault();

            // Eğer metod üzerinde ValidationAttribute yoksa
            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            // Cache key üretimi: MethodName(param1,param2,...)
            if (invocation.Arguments.Length > 0 && invocation.Arguments[0] is LambdaExpression filterExpr)
            {
                var filters = ExpressionFilterParser.ExtractFilters(filterExpr);
                var filterKey = string.Join(",", filters.Select(kv => $"{kv.Key}={kv.Value}"));

                var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";
                var arguments = invocation.Arguments.Skip(1).Select(x => x?.ToString() ?? "<Null>");
                key = $"{methodName}({filterKey};{string.Join(",", arguments)})";

                // key → cache lookup
            }
            else
            {
                var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";
                var arguments = invocation.Arguments.ToList();
                key = $"{methodName}({string.Join(",", arguments)})";

            }

            //$"{methodName}({string.Join(",", arguments.Select(x => x is Expression ? "<expr>" : x?.ToString() ?? "<Null>"))})"; //Expression yada IQueryable gibi runtime bazlı parametreleri key üretiminde igronere ediyoruz.

            // Eğer cache varsa, method çağrısı yapmadan dön
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }

            // Methodu çalıştır
            invocation.Proceed();

            // Dönen sonucu cache’e ekle
            _cacheManager.Add(key, invocation.ReturnValue, _durationTime);
        }
    }

}
