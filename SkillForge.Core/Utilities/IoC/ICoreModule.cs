using Microsoft.Extensions.DependencyInjection;

namespace SkillForge.Core.Utilities.IoC
{
    //.Net core taraflı servisleri yöneteceğimiz framework taraflı arayüzü
    public interface ICoreModule {
        void Load(IServiceCollection services);
    }
}