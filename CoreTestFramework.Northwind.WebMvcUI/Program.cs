using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.Concrate;
using CoreTestFramework.Northwind.DataAccess.Concrate;
//Proje servis conteineri kapsayıcı servis ağı
var builder = WebApplication.CreateBuilder(args);

//mvc, restapi, razorpages şablonları ile çalışabiliriz. Hangi şablon ile çalışacaksak belirtiyoruz.
builder.Services.AddRazorPages();

builder.Services.AddTransient<ProductDAL>();
builder.Services.AddSingleton<IProductService, ProductManager>();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

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
