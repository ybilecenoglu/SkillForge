using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Concrate;
using CoreTestFramework.Northwind.WebMvcUI.Common;
using DataTables.AspNet.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//mvc, restapi, razorpages şablonları ile çalışabiliriz. Hangi şablon ile çalışacaksak belirtiyoruz.
builder.Services.AddRazorPages();

builder.Services.AddTransient<NorthwindContext>();
builder.Services.AddSingleton<ProductDAL>();
builder.Services.AddSingleton<IProductService, ProductManager>();

// builder.Services.AddControllersWithViews();

// builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.RegisterDataTables();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// builder.Services.AddDbContext<NorthwindContext>(option => option.UseNpgsql(
//     builder.Configuration.GetConnectionString("NorthwindContext")
// ));

var app = builder.Build();


// app.UseHttpsRedirection();

//wwwroot klasörü altında 
//css
//js
//lib
app.UseStaticFiles();
//MiddleWeare aktif edilmesi için kullanılan komut.
app.UseRouting();

// app.UseAuthorization();

#region Default Routing Yapısı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");
#endregion

app.MapRazorPages();

app.Run();
