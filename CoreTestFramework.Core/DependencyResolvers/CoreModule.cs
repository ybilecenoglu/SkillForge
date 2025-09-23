using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft;
using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTestFramework.Core.DependencyResolvers
{
    //Merkezi dependecy configurasyonlarini uygulayacağımız nesnemiz
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache(); //.Net Core default memory cache aktif ediyoruz.
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
        
        }
    }
}