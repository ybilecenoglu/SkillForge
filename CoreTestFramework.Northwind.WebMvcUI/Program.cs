using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.WebMvcUI.Common;
using DataTables.AspNet.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Database Connection
// builder.Services.AddDbContext<NorthwindContext>(options => {
//     var config = builder.Configuration;
//     var connectionString =config.GetConnectionString("NorthwindContext");
//     options.UseNpgsql(connectionString);
// });

//mvc, restapi, razorpages şablonları ile çalışabiliriz. Hangi şablon ile çalışacaksak belirtiyoruz.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NorthwindContext>();
builder.Services.AddSingleton<IProductDAL, ProductDAL>();
builder.Services.AddSingleton<IProductService, ProductManager>();

builder.Services.AddSingleton<ICategoryDAL, CategoryDAL>();
builder.Services.AddSingleton<ICategoryService, CategoryManager>();

//JSON serileştirmesini yapılandırmaası lowercase için
builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.RegisterDataTables();
builder.Services.AddSession();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// builder.Services.AddDbContext<NorthwindContext>(option => option.UseNpgsql(
//     builder.Configuration.GetConnectionString("NorthwindContext")
// ));

var app = builder.Build();

app.UseHttpsRedirection();
//wwwroot klasörü altında 
//css
//js
//lib
app.UseStaticFiles(new StaticFileOptions(){
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot")), RequestPath="/CoreTestFramework.Northwind.WebMvcUI/wwwroot"
}); //wwwroot klasöründe dışarıdan erişmek için staticfile configürasyonu.
app.UseRouting();
app.UseAuthorization();

//Npgsql Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported hatası düzeltmek için kullanılan configurasyon.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); 
#region Default Routing Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");
#endregion

app.UseSession();
app.MapRazorPages();

app.Run();
