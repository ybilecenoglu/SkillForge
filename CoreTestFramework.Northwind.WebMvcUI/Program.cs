using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft;
using CoreTestFramework.Core.DependencyResolvers;
using CoreTestFramework.Core.Extensions;
using CoreTestFramework.Core.Utilities.IoC;
using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.WebMvcUI.Common;
using DataTables.AspNet.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// Database Connection
// builder.Services.AddDbContext<NorthwindContext>(options => {
//     var config = builder.Configuration;
//     var connectionString =config.GetConnectionString("NorthwindContext");
//     options.UseNpgsql(builder.Configuration["ConnectionStrings:NorthwindContext]);
// });

//mvc, restapi, razorpages şablonları ile çalışabiliriz. Hangi şablon ile çalışacaksak belirtiyoruz.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NorthwindContext>();
builder.Services.AddSingleton<IProductDAL, ProductDAL>();
builder.Services.AddSingleton<IProductService, ProductManager>();

builder.Services.AddSingleton<ICategoryDAL, CategoryDAL>();
builder.Services.AddSingleton<ICategoryService, CategoryManager>();
builder.Services.AddSingleton<ICacheManager, MemoryCacheManager>();

//JSON serileştirmesini yapılandırmaası lowercase için
builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.RegisterDataTables();
builder.Services.AddSession();
builder.Services.AddAutoMapper(typeof(MappingProfile));
//.Net core tarafındaki framework seviyesinde bütün merkezi servis injectionlarını burada eklemiş olduk.
builder.Services.AddDependencyResolvers(new ICoreModule[] {
    new CoreModule()
});

var app = builder.Build();
// app.UseHttpsRedirection();
//wwwroot klasörü altında 
//css
//js
//lib
// app.UseStaticFiles(new StaticFileOptions(){
//     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot")), RequestPath="/CoreTestFramework.Northwind.WebMvcUI/wwwroot"
// }); //wwwroot klasöründe dışarıdan erişmek için staticfile configürasyonu.
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//Npgsql Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported hatası düzeltmek için kullanılan configurasyon.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); 
#region Default Routing Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");
    //app.MapDefaultControllerRoute(); Default roting yapısı için bu methodu kullanabiliriz.
#endregion
#region Custom Routing
// app.MapControllerRoute(
//     name: "customize_name",
//     pattern: "product/{url}",
//     defaults: new {controller = "Product", action="Details"}
// );
#endregion

app.UseSession();
app.MapRazorPages();

app.Run();
