using System.Linq.Expressions;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Business
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDAL _categoryDAL;

        public CategoryManager(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }
        public async Task<Result> AddCategoryAsync(Category category)
        {
            var add_result = await _categoryDAL.AddAsync(category);
            return add_result;
        }

        public async Task<Result> DeleteCategoryAsync(Category category)
        {
            var delete_result = await _categoryDAL.DeleteAsync(category);
            return delete_result;
        }

        public async Task<Result<Category>> GetCategoryAsync(int id)
        {
            var get_category_result = await _categoryDAL.FindByIdAsync(id);
            return get_category_result;
        }

        public async Task<Result<List<Category>>> GetCategoryListAsync(Expression<Func<Category, bool>> filter = null)
        {
            var get_category_list_result = await _categoryDAL.GetListAsync(filter);
            return get_category_list_result;
        }

        public async Task<Result> UpdateCategoryAsync(Category category)
        {
            var get_update_result = await _categoryDAL.UpdateAsync(category);
            return get_update_result;
        }
    }
}