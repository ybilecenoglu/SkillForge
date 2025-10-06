using Microsoft.AspNetCore.Mvc;

namespace SkillForge.WebMvcUI.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
