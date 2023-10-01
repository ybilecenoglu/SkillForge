using Microsoft.AspNetCore.Mvc;

namespace CoreTestFramework.Northwind.WebMvcUI.Controllers
{
    public class HomeController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
