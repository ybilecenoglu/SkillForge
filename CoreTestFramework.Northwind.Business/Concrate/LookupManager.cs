using System.Linq.Expressions;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Business
{
    public class LookupManager : ILookupService
    {
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        public LookupManager(ICategoryService categoryService, ISupplierService supplierService)
        {
            _categoryService = categoryService;
            _supplierService = supplierService;
        }
        public async Task<Result<List<Category>>> GetLookupCategoriesAsync(Expression<Func<Category, bool>> expression)
        {
            var categories_result = await _categoryService.GetCategoryListAsync(expression);
            return categories_result;
        }

        public Task<Result<List<Product>>> GetLookupProductsAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<Supplier>>> GetLookupSuppliersAsync(Expression<Func<Supplier, bool>> expression)
        {
            var supplier_result = await _supplierService.GetSupplierListAsync(expression);
            return supplier_result;
        }
    }
}