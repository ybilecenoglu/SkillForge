using System.Linq.Dynamic;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Entities.Concrate;
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

        public async Task<IActionResult> Index(ProductViewModel vm = null)
        {
            var data = await _productService.GetProductListAsync();
            return View();
        }

        [HttpPost]
        public IActionResult PageData()
        {
            return Json("");
        }
    }
}
