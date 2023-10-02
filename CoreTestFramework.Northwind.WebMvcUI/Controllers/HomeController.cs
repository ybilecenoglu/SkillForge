using Microsoft.AspNetCore.Mvc;

namespace CoreTestFramework.Northwind.WebMvcUI.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
