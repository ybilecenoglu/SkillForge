using AutoMapper;
using CoreTestFramework.Core.Common;
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
using Newtonsoft.Json;
using static CoreTestFramework.Northwind.WebMvcUI.Extension.QueryableExtension;
using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Core;
using CoreTestFramework.Northwind.Entities.ValidationRules.FluentValidation;

namespace CoreTestFramework.Northwind.WebMvcUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        
        private readonly IMapper _mapper;
        private readonly NorthwindContext _northwindContext;
        public ProductController(IProductService productService, IMapper mapper, NorthwindContext northwindContext)
        {
            _productService = productService;
            _mapper = mapper;
            _northwindContext = northwindContext;
        }
        public async Task<IActionResult> Index(ProductViewModel vm = null)
        {
            var result = new Result { Success = false };
            try
            {
                var categories = await _northwindContext.Categories.ToListAsync();
                vm.Categories = new SelectList(categories, "category_id", "category_name", vm.CategoryID);
                var suppliers = await _northwindContext.Suppliers.ToListAsync();
                vm.Suppliers = new SelectList(suppliers, "supplier_id", "company_name", vm.SupplierID);

                return View(vm);
            }
            catch (System.Exception ex)
            {
                result.Success = true;
                result.Message = ex.Message;
                TempData["result"] = JsonConvert.SerializeObject(result);

                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PageData(IDataTablesRequest request, int categoryID, int supplierID)
        {
            Result result = new Result { Success = false };
            IQueryable<ProductDTO> products;
            Result<List<Product>> product_result = new Result<List<Product>>();
            DataTablesResponse response;
            try
            {
                if (request == null)
                {
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    response = DataTablesResponse.Create(request, 0, 0, new List<Product>());
                    return new DataTablesJsonResult(response, true);
                }
                if (supplierID > 0 && categoryID > 0)
                {
                    product_result = await _productService.GetProductListAsync(p => p.supplier_id == supplierID && p.category_id == categoryID, p => p.Category, p => p.Supplier);
                }
                else if (supplierID > 0)
                {
                    product_result = await _productService.GetProductListAsync(p => p.supplier_id == supplierID, p => p.Category, p => p.Supplier);
                }
                else if (categoryID > 0)
                {
                    product_result = await _productService.GetProductListAsync(p => p.category_id == categoryID, p => p.Category, p => p.Supplier);
                }
                else
                    product_result = await _productService.GetProductListAsync(null, p => p.Category, p => p.Supplier);


                if (product_result.Success == false)
                {
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    response = DataTablesResponse.Create(request, 0, 0, new List<Product>());
                    return new DataTablesJsonResult(response, true);
                }

                var mapped_list_product = _mapper.Map<List<ProductDTO>>(product_result.Data);
                products = mapped_list_product.AsQueryable();

                if (!string.IsNullOrEmpty(request.Search.Value))
                {
                    var searchToUpperValue = request.Search.Value.Trim().ToUpper();
                    var searchValue = request.Search.Value.Trim();

                    products = products.Where(p => p.product_name.ToUpper().Contains(searchToUpperValue) || p.product_name.Contains(searchValue));
                }

                var dataPage = products;
                var orderColumns = request.Columns.Where(c => c.IsSortable == true && c.Sort != null);
                var searchColumns = request.Columns.Where(c => c.IsSearchable);

                if (orderColumns.Count() == 0)
                {
                    dataPage = dataPage.Take(request.Length);
                }
                else
                {
                    if (request.Length > 0)
                    {
                        dataPage = dataPage.OrderBy(orderColumns).Skip(request.Start).Take(request.Length);
                    }
                    else
                    {
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
            var result = new Result { Success = false };
            try
            {
                if (id == 0)
                {
                    result.Success = false;
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return Json(result);
                }

                var product = await _productService.GetFindByIdAsync(id);
                if (product != null)
                {
                    product.Data.is_deleted = true;
                    var delete_product_result = await _productService.UpdateProductAsync(product.Data);
                    if (delete_product_result.Success == true)
                    {
                        result.Success = true;
                        result.Message = "Silme işlemi başarıyla gerçekleşti";
                        return Json(result);
                    }
                    else
                    {
                        result.Success = delete_product_result.Success;
                        result.Message = delete_product_result.Message;
                        return Json(result);
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Sistemde kayıtlı ürün bulunamadı.";
                    return Json(result);
                }

            }
            catch (System.Exception)
            {
                result.Success = false;
                result.Message = "Sistemde bir veya daha fazla hata oluştu";
            }

            return Json(result);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = new Result { Success = false };
            var product_vm = new ProductViewModel();
            try
            {
                if (id == 0)
                {
                    result.Success = false;
                    result.Message = "Seçili ürün bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }
                var product = await _productService.GetFindByIdAsync(id);
                if (product.Success == true && product.Data != null)
                {
                    product_vm.CategoryID = product.Data.category_id;
                    product_vm.SupplierID = product.Data.supplier_id;
                    var mapped_product = _mapper.Map<ProductDTO>(product.Data);
                    product_vm.Product = mapped_product;

                    var categories = await _northwindContext.Categories.ToListAsync();
                    product_vm.Categories = new SelectList(categories, "category_id", "category_name", product_vm.CategoryID);
                    var suppliers = await _northwindContext.Suppliers.ToListAsync();
                    product_vm.Suppliers = new SelectList(suppliers, "supplier_id", "company_name", product_vm.SupplierID);

                }
            }
            catch (System.Exception)
            {

                result.Message = "Sistemde bir veya daha fazla hata oluştu";
                //TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Index");
            }

            return PartialView(product_vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel vm = null)
        {
            var result = new Result { Success = false };
            try
            {
                var get_product_result = await _productService.GetFindByIdAsync(vm.Product.product_id);

                if (get_product_result.Success && get_product_result.Data != null)
                {
                    get_product_result.Data.product_name = vm.Product.product_name;
                    get_product_result.Data.supplier_id = vm.SupplierID;
                    get_product_result.Data.category_id = vm.CategoryID;
                    get_product_result.Data.quantity_per_unit = vm.Product.quantity_per_unit;
                    get_product_result.Data.unit_price = vm.Product.unit_price;
                    get_product_result.Data.units_in_stock = vm.Product.units_in_stock;
                    get_product_result.Data.reorder_level = vm.Product.reorder_level;
                    get_product_result.Data.units_on_order = vm.Product.units_on_order;

                    if (vm.Product.AktifMi == true)
                    {
                        get_product_result.Data.discontinued = 1;
                    }
                    else
                        get_product_result.Data.discontinued = 0;

                    var update_result = await _productService.UpdateProductAsync(get_product_result.Data);

                    if (update_result.Success == true)
                    {
                        result.Success = true;
                        result.Message = "Güncelleme işlemi başarıyla gerçekleşti";
                        return Json(result);
                    }
                    else
                    {
                        result.Message = update_result.Message;
                        return Json(result);
                    }
                }
                else
                {
                    result.Message = "Update edilecek ürün bulunamadı.";
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
            catch (System.Exception)
            {
                result.Message = "Sistemde bir veya daha fazla hata oluştu";
                //TempData["result"] = JsonConvert.SerializeObject(result);
                return Json(result);
            }
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductViewModel();
            try
            {
                var categories = await _northwindContext.Categories.ToListAsync();
                viewModel.Categories = new SelectList(categories, "category_id", "category_name", viewModel.CategoryID);
                var suppliers = await _northwindContext.Suppliers.ToListAsync();
                viewModel.Suppliers = new SelectList(suppliers, "supplier_id", "company_name", viewModel.SupplierID);
            }
            catch (System.Exception)
            {
                TempData["result"] = JsonConvert.SerializeObject("Sistemde bir veya daha fazla hata oluştu");
                return RedirectToAction("Index");
            }
            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm = null)
        {
            var result = new Result { Success = false };
            try
            {
                var product = new Product
                {
                    product_name = vm.Product.product_name,
                    category_id = vm.CategoryID,
                    supplier_id = vm.SupplierID,
                    quantity_per_unit = vm.Product.quantity_per_unit,
                    unit_price = vm.Product.unit_price,
                    units_in_stock = vm.Product.units_in_stock,
                    units_on_order = vm.Product.units_on_order,
                    reorder_level = vm.Product.reorder_level,
                    discontinued = vm.Product.AktifMi == true ? 1 : 0
                };

                var add_product_result = await _productService.AddProductAsync(product);
                if (add_product_result.Success == false)
                {
                    result.Success = false;
                    result.Message = add_product_result.Message;

                    return Json(result);
                }


            }
            catch (ValidationException validationEx)
            {
                validationEx.Errors.ToList().ForEach(error =>
                {
                    result.Messages.Add(error.ErrorMessage);
                });

                return Json(result);
            }
            catch (System.Exception)
            {
                result.Success = false;
                result.Message = "Sistemde bir veya daha fazla hata oluştu";

                return Json(result);
            }

            result.Success = true;
            result.Message = "Ürün ekleme işlemi başarıyla gerçekleşti.";
            return Json(result);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = new Result { Success = false };
            var product_vm = new ProductViewModel();
            try
            {
                if (id == 0)
                {
                    result.Message = "Aranan ürün bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }
                var product_result = await _productService.GetProductAsync(p => p.product_id == id, p => p.OrderDetails, p => p.Supplier);
                if (product_result != null)
                {
                    var mapped_product = _mapper.Map<ProductDTO>(product_result.Data);
                    ViewBag.ProductID = product_result.Data.product_id;
                    product_vm.Product = mapped_product;
                }

                return PartialView(product_vm);

            }
            catch (System.Exception)
            {
                result.Message = "Sistemde bir veya daha fazla hata oluştu";

                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetDataTableOrderDetail(IDataTablesRequest request, int id)
        {
            var result = new Result { Success = false };
            IQueryable<OrderDetail> orderDetails;
            DataTablesResponse response;
            try
            {
                if (request == null)
                {
                    result.Message = "İstek elde edilemedi";
                    return Json(result);
                }

                //DbContext üzerinden oluşturduğumuz function çağırıyoruz.

                orderDetails = _northwindContext.OrderDetails.Where(x => x.product_id == id);
                if (!string.IsNullOrEmpty(request.Search.Value))
                {
                    int searchValue = Convert.ToInt32(request.Search.Value);
                    orderDetails = orderDetails.Where(od => od.order_id == searchValue);
                }
                var orderColumns = request.Columns.Where(c => c.IsSortable == true && c.Sort != null);
                var searchColumns = request.Columns.Where(c => c.IsSearchable);
                var dataPage = orderDetails;
                if (dataPage.Count() == 0)
                {
                    dataPage = dataPage.Take(request.Length);
                }
                else
                {
                    if (request.Length > 0)
                    {
                        dataPage = dataPage.OrderBy(orderColumns).Skip(request.Start).Take(request.Length);
                    }
                    else
                    {
                        dataPage = dataPage.OrderBy(orderColumns);
                    }
                }
                response = DataTablesResponse.Create(request, orderDetails.Count(), orderDetails.Count(), dataPage);
                return new DataTablesJsonResult(response, true);


            }
            catch (System.Exception)
            {
                result.Message = "Sistemde bir veya daha fazla hata oluştu";
                response = DataTablesResponse.Create(request, 0, 0, null);
                return new DataTablesJsonResult(response, true);
            }
        }
        public async Task<IActionResult> _OrderDetail(int id)
        {
            var result = new Result { Success = false };
            var product_vm = new ProductViewModel();
            try
            {
                if (id == 0)
                {
                    result.Message = "Sistemde bir veya daha fazla hata oluştu.";
                    return Json(result);
                }

                //Order tablosundan order_id ye göre ilgili order getiren db function çağırıyoruz.
                var order = await _northwindContext.GetOrderWithOrderId(id).FirstOrDefaultAsync();

                //Eager loading
                //Call postgresql function fromsqlinterpolated
                // var order = await _northwindContext.Orders.FromSqlInterpolated($"select * from fc_get_orderdetails_with_productId({id})")
                // .Include(o => o.Customer)
                // .Include(o => o.Employee)
                // .Include(o => o.Shipper)
                // .AsNoTracking()
                // .FirstAsync();

                //Explict loading
                // await _northwindContext.Entry(order).Reference(o => o.Customer).LoadAsync();
                // await _northwindContext.Entry(order).Reference(o => o.Employee).LoadAsync();
                // await _northwindContext.Entry(order).Reference(o => o.Shipper).LoadAsync();

                if (order == null)
                {
                    result.Message = "Seçili siparişle ilgili sistemde kayıtlı detay bulunamadı.";
                    return Json(result);
                }

                product_vm.Order = order;

                return PartialView(product_vm);

            }
            catch (System.Exception)
            {
                result.Message = "Sistemde bir veya daha fazla hata oluştu.";
                return Json(result);
            }
        }
    }
}
