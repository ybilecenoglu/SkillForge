using System.Web.Helpers;
using AutoMapper;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.Entities.DTO;
using CoreTestFramework.Northwind.WebMvcUI.Extension;
using CoreTestFramework.Northwind.WebMvcUI.ViewModels;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static CoreTestFramework.Northwind.WebMvcUI.Extension.QueryableExtension;

namespace CoreTestFramework.Northwind.WebMvcUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private readonly NorthwindContext _northwindContext;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper, NorthwindContext northwindContext)
        {
            _productService = productService;
            _northwindContext = northwindContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(ProductViewModel vm = null)
        {
            var result = new Result { Success = false};
            try
            {
                var categories = await _northwindContext.Categories.ToListAsync();
                vm.Categories = new SelectList(categories, "CategoryID","CategoryName", vm.CategoryID);
                var suppliers = await _northwindContext.Suppliers.ToListAsync();
                vm.Suppliers = new SelectList(suppliers, "SupplierID", "CompanyName", vm.SupplierID);
                
                return View(vm);
            }
            catch (System.Exception ex)
            {
                result.Success = true;
                result.Message = ex.Message;
                TempData["result"] =result;

                return View(vm);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PageData(IDataTablesRequest request, int categoryID, int supplierID)
        {
            Result result = new Result { Success = false, };
            IQueryable<ProductDTO> products;
            Result<List<Product>> product_result = new Result<List<Product>>();
            DataTablesResponse response;
            try
            {
                if (request == null)
                {
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = result;
                    response = DataTablesResponse.Create(request, 0, 0, new List<Product>());
                    return new DataTablesJsonResult(response,true);
                }

                if (supplierID > 0 && categoryID > 0)
                {
                    product_result = await _productService.GetProductListAsync(p => p.SupplierID == supplierID && p.CategoryID == categoryID);
                }
                else if(supplierID > 0)
                {
                    product_result = await _productService.GetProductListAsync(p => p.SupplierID == supplierID);
                }   
                else if(categoryID > 0){
                   product_result = await _productService.GetProductListAsync(p => p.CategoryID == categoryID);
                }
                else
                    product_result = await _productService.GetProductListAsync();

                if (product_result.Success == false)
                {
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = result;
                    response = DataTablesResponse.Create(request, 0, 0, new List<Product>());
                    return new DataTablesJsonResult(response,true);
                }

                var mapped_list_product = _mapper.Map<List<ProductDTO>>(product_result.Data);
                products = mapped_list_product.AsQueryable();
                if (!string.IsNullOrEmpty(request.Search.Value))
                {
                    var searchToUpperValue = request.Search.Value.Trim().ToUpper();
                    var searchValue = request.Search.Value.Trim();

                    products = products.Where(p => p.ProductName.ToUpper().Contains(searchToUpperValue) || p.ProductName.Contains(searchValue));
                }

                var dataPage = products;
                var orderColumns = request.Columns.Where(c => c.IsSortable == true && c.Sort != null);
                var searchColumns = request.Columns.Where(c => c.IsSearchable);

                if (orderColumns.Count() == 0)
                {
                    dataPage = dataPage.Take(request.Length);
                }
                else {
                    if (request.Length > 0)
                    {
                        dataPage = dataPage.OrderBy(orderColumns).Skip(request.Start).Take(request.Length);
                    }
                    else{
                        dataPage = dataPage.OrderBy(orderColumns);
                    }
                }
                response = DataTablesResponse.Create(request, products.Count(), products.Count(), dataPage);
                return new DataTablesJsonResult(response, true);
            }
            catch (System.Exception ex)
            {
                string message = ex.Message;
                throw;
            }
            
        }
        public async Task<ActionResult> Delete(int id) 
        {
            var result = new Result { Success = false};
            try
            {
                if(id == null){
                    result.Success =false;
                    result.Message = "Aranan ürün bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return Json(result);
                }

                var product = await _productService.GetProductAsync(id);
                if(product != null){
                    var delete_product_result = await _productService.DeleteProductAsync(product.Data);
                    if(delete_product_result.Success == true){
                        result.Success = true;
                        result.Message = $"{product.Data.ProductName} silme işlemi başarıyla gerçekleşti";
                        return Json(result);
                    }
                    else{
                        result.Success =delete_product_result.Success;
                        result.Message = delete_product_result.Message;
                        return Json(result);
                        
                    }
                }
                else{
                    result.Success = false;
                    result.Message ="Ürün bulunamadı.";
                    return Json(result);
                }

            }
            catch (System.Exception ex)
            {
                result.Success = false;
                result.Message = "Ürün silme işlemi başarısız oldu";
            }

            return Json(result);
        }
        public async Task<IActionResult> Edit(int id)
        {
             var result = new Result { Success = false};
             var product_vm = new ProductViewModel();
             try
             {
                if(id == null){
                    result.Success =false;
                    result.Message = "Seçili ürün bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }
                 var product = await _productService.GetProductAsync(id);
                 if(product.Success == true && product.Data != null){

                    product_vm.CategoryID = product.Data.CategoryID;
                    product_vm.SupplierID = product.Data.SupplierID;
                    var mapped_product = _mapper.Map<ProductDTO>(product.Data);
                    product_vm.Product = mapped_product;
                    
                    var categories = await _northwindContext.Categories.ToListAsync();
                    product_vm.Categories = new SelectList(categories, "CategoryID","CategoryName", product_vm.CategoryID);
                    var suppliers = await _northwindContext.Suppliers.ToListAsync();
                    product_vm.Suppliers = new SelectList(suppliers, "SupplierID","CompanyName", product_vm.SupplierID);
                    
                    return PartialView(product_vm);
                 }
                 
             }
             catch (System.Exception ex)
             {
                result.Success =false;
                result.Message = ex.Message;
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Index");
             }
             
             return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel vm = null)
        {
            var result = new Result{ Success = false};
            try
            {
                    var get_product_result = await _productService.GetProductAsync(vm.Product.ProductID);
                    if (get_product_result.Success && get_product_result.Data != null)
                    {
                        get_product_result.Data.ProductName = vm.Product.ProductName;
                        get_product_result.Data.SupplierID = vm.SupplierID;
                        get_product_result.Data.CategoryID = vm.CategoryID;
                        get_product_result.Data.QuantityPerUnit = vm.Product.QuantityPerUnit;
                        get_product_result.Data.UnitPrice = vm.Product.UnitPrice;
                        get_product_result.Data.UnitsInStock = vm.Product.UnitsInStock;
                        get_product_result.Data.ReorderLevel = vm.Product.ReorderLevel;
                        get_product_result.Data.UnitsOnOrder = vm.Product.UnitsOnOrder;
                        if(vm.Product.AktifMi == true){
                           get_product_result.Data.Discontinued = "1";
                        }
                        else
                           get_product_result.Data.Discontinued = "0";
                        
                        
                        var update_result = await _productService.UpdateProductAsync(get_product_result.Data);
                        if (update_result.Success == false)
                        {
                           result.Message = update_result.Message;
                           TempData["result"] = JsonConvert.SerializeObject(result);
                           return Json(result);
                        }
                    }
                    else {
                        result.Message = "Güncellenecek ürün bulunamadı";
                        TempData["result"] = JsonConvert.SerializeObject(result);
                        return Json(result);
                    }
            }
            catch (ValidationException validationEx)
            {
                    foreach (var error in validationEx.Errors)
                    {
                        result.Messages.Add(error.ErrorMessage);
                    }
                    return Json(result);
            }
            catch (System.Exception ex)
            {
                result.Message = ex.Message;
                TempData["result"] = JsonConvert.SerializeObject(result);
                return Json(result);
            }
             
             result.Success = true;
             result.Message = $"{vm.Product.ProductName} güncelleme işlemi başarıyla gerçekleşti";
             
             return Json(result);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductViewModel();
            try
            {
                var categories = await _northwindContext.Categories.ToListAsync();
                viewModel.Categories = new SelectList(categories, "CategoryID","CategoryName", viewModel.CategoryID);
                var suppliers = await _northwindContext.Suppliers.ToListAsync();
                viewModel.Suppliers = new SelectList(suppliers, "SupplierID", "CompanyName", viewModel.SupplierID);
            }
            catch (System.Exception)
            {
                throw;
            }
            return PartialView(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel vm = null)
        {
            var result = new Result {Success = false};
            try
            {
                var product = new Product {
                    ProductName = vm.Product.ProductName,
                    CategoryID = vm.CategoryID,
                    SupplierID = vm.SupplierID,
                    QuantityPerUnit = vm.Product.QuantityPerUnit,
                    UnitPrice = vm.Product.UnitPrice,
                    UnitsInStock = vm.Product.UnitsInStock,
                    UnitsOnOrder = vm.Product.UnitsOnOrder,
                    ReorderLevel = vm.Product.ReorderLevel,
                    Discontinued = vm.Product.AktifMi == true ? "1" : "0"
                };

                var add_product_result = await _productService.AddProductAsync(product);
                if (add_product_result.Success == false)
                {
                    result.Success = false;
                    result.Message = add_product_result.Message;
                    // TempData["result"] = JsonConvert.SerializeObject(result);
                    return Json(result);
                }
            }
            catch(ValidationException validationEx){
                    
                    foreach (var error in validationEx.Errors)
                    {
                        result.Messages.Add(error.ErrorMessage);
                    }
                    return Json(result);

            }
            catch (System.Exception  ex)
            {
                    result.Success = false;
                    result.Message = ex.Message;
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return Json(result);
            }
            
            result.Success = true;
            result.Message = $"{vm.Product.ProductName} kaydı başarıyla eklendi.";
            // TempData["result"] = JsonConvert.SerializeObject(result);
           return Json(result);
        }
        public async Task<IActionResult> Detail(int id) {
            var result = new Result { Success = false};
            var product_vm = new ProductViewModel();
            try
            {
                if (id == 0)
                {
                    result.Message = "Aranan ürün bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }

                var product_result = await _productService.GetProductAsync(id);
                if (product_result.Success == true && product_result.Data != null)
                {
                    var mapped_product = _mapper.Map<ProductDTO>(product_result.Data);
                    product_vm.Product = mapped_product;
                }

                return PartialView(product_vm);

            }
            catch (System.Exception ex)
            {
                result.Message = ex.Message;
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Index");
            }
        }
    }

}
