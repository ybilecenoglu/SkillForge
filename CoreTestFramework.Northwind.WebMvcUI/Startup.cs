using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using DataTables.AspNet.AspNetCore;
namespace CoreTestFramework.Northwind.WebMvcUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddTransient<ProductDAL>();
            services.AddSingleton<IProductService, ProductManager>();
            
            // DataTables.AspNet registration with default options.
            services.RegisterDataTables();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Adds dev exception page for better debug experience.
            app.UseDeveloperExceptionPage();

            // Add MVC to the request pipeline.
            app.UseRouting();
            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
             }
            );
        }
    }
}