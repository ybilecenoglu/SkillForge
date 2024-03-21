using Microsoft.Extensions.DependencyInjection;

namespace CoreTestFramework.Core.Utilities.IoC
{
    public interface ICoreModule {
        void Load(IServiceCollection services);
    }
}