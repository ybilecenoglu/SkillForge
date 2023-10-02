using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.WebMvcUI.Models;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
namespace CoreTestFramework.Northwind.WebMvcUI.Controllers
{
    
    public class ProductController : Controller
    {
        private IProductService _productService;
        public ProductController(IProductService productService) 
        {
            _productService = productService;
        }

        public  IActionResult Index(ProductViewModel vm)
        {
            return View();
        }

        
        [Route("Product/PageData")]
        [HttpPost]
        public JsonResult PageData(IDataTablesRequest request, string publisherRecId)
        {
            var a = request;
            return Json("");
        }
    }
}
