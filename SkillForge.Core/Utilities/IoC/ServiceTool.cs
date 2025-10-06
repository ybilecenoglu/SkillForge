using Microsoft.Extensions.DependencyInjection;

namespace SkillForge.Core.Utilities.IoC
{
    //Uygulamamımızın ServiceCollection erişmek için kullanacağımız tool.
    public static class ServiceTool 
    {
        //ServiceProvider erişmek için kullanacağımız propertimiz.
        public static IServiceProvider ServiceProvider {get; set;}
        //.Net Core middleware tarafında (program.cs) eklenen servislere ulaşacağımız method.
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}