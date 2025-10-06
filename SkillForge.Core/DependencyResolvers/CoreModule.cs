using SkillForge.Core.CrossCuttingConcern.Caching;
using SkillForge.Core.CrossCuttingConcern.Caching.Microsoft;
using SkillForge.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace SkillForge.Core.DependencyResolvers
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