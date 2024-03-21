using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp.Caching
{
    [PSerializable]//Bir türe uygulandığında PostSharp'ın PortableFormatter tarafından kullanılmak üzere bir serileştirici oluşturmasına neden olan özel öznitelik.
    public class CacheAspect : MethodInterceptionAspect
    {
        private int _durationTime;
        private ICacheManager _cacheManager;

        public CacheAspect(int durationTime=60)
        {
            _durationTime = durationTime;
            //_cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var a = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            var methodName = $"{args.Method.ReflectedType.FullName}.{args.Method.Name}";
            var arguments = args.Arguments.ToList();
            var key =$"{methodName}({string.Join(",", arguments.Select(x => x?.ToString()??"<Null>"))})";

            if (a.IsAdd(key))
            {
                args.ReturnValue = a.Get(key);
                return;
            }

            args.Proceed();
            a.Add(key,args.ReturnValue, _durationTime);

        }

    }
}