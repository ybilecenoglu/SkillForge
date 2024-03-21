using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft;
using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTestFramework.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
        }
    }
}