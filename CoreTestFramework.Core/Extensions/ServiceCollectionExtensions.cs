using CoreTestFramework.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTestFramework.Core.Extensions
{
    //Merkezi servisleri eklediğimiz nesnemiz.
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] modules){
            foreach (var module in modules)
            {
                module.Load(services); //CoreModule nesnesinden gelen tüm modülleri ekliyoruz.
            }
            return ServiceTool.Create(services); //Gelen tüm servisleri ServiceTool aracılığı ile yapılandıran ve geriye ServiceCollection döndüren operasyon
        }
    }
}