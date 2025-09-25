using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using CoreTestFramework.Core.Aspect.Autofac;
using CoreTestFramework.Core.CrossCuttingConcern.Caching;
using CoreTestFramework.Core.DependencyResolvers;
using CoreTestFramework.Core.Extensions;
using CoreTestFramework.Core.Utilities.IoC;
using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.Entities.ValidationRules.FluentValidation;
using CoreTestFramework.Northwind.DataAccess;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.WebMvcUI.Common;
using DataTables.AspNet.AspNetCore;
using FluentValidation;
using CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft;

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
builder.Services.AddSingleton<ILookupService, LookupManager>();
builder.Services.AddSingleton<ISupplierDAL, SupplierDAL>();
builder.Services.AddSingleton<ISupplierService, SupplierManager>();
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
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
//.Net core tarafındaki framework seviyesinde bütün merkezi servis injectionlarını burada eklemiş olduk.
builder.Services.AddDependencyResolvers(new ICoreModule[] {
    new CoreModule()
});

// Use Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    
    // MemoryCacheManager register
    containerBuilder.RegisterType<MemoryCacheManager>()
        .As<ICacheManager>()
        .SingleInstance();

    // CacheInterceptor register
    containerBuilder.RegisterType<CacheInterceptor>()
        .AsSelf();

    // CacheRemoveInterceptor register
    containerBuilder.RegisterType<CacheRemoveInterceptor>()
        .AsSelf();

    // Validator register
    containerBuilder.RegisterType<ProductValidation>()
        .As<IValidator<Product>>() // DI container için IValidator<Product>
        .SingleInstance();

    // Generic interceptor register
    containerBuilder.RegisterGeneric(typeof(FluentValidationInterceptor<,>)).AsSelf();

    
    // Service register ve interceptor bağla
    containerBuilder.RegisterType<ProductManager>()
        .As<IProductService>()
        .EnableInterfaceInterceptors()
        .InterceptedBy(typeof(FluentValidationInterceptor<IValidator<Product>, Product>))
        .InterceptedBy(typeof(CacheInterceptor))
        .InterceptedBy(typeof(CacheRemoveInterceptor));

    containerBuilder.RegisterType<LookupManager>()
        .As<ILookupService>()
        .EnableInterfaceInterceptors()
        .InterceptedBy(typeof(CacheInterceptor))
        .InterceptedBy(typeof(CacheRemoveInterceptor));
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
