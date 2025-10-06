using System.Linq.Expressions;
using SkillForge.Core.Common;
using SkillForge.Business;
using SkillForge.Entities.Model;

namespace SkillForge.Business
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