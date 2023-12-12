using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.WebMvcUI.Common;
using DataTables.AspNet.AspNetCore;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

//Database Connection
// builder.Services.AddDbContext<NorthwindContext>(options => {
//     var config = builder.Configuration;
//     var connectionString =config.GetConnectionString("NorthwindContext");
//     options.UseSqlite(connectionString);
// });

//mvc, restapi, razorpages şablonları ile çalışabiliriz. Hangi şablon ile çalışacaksak belirtiyoruz.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<NorthwindContext>();
builder.Services.AddSingleton<ProductDAL>();
builder.Services.AddSingleton<IProductService, ProductManager>();
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
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
#region Default Routing Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");
#endregion

app.UseSession();
app.MapRazorPages();

app.Run();
