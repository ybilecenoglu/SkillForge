using System.Linq.Expressions;
using CoreTestFramework.Core;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Business
{
    public interface ICategoryService 
    {
        [Cache]
        Task<Result<List<Category>>> GetCategoryListAsync(Expression<Func<Category, bool>> filter = null);
        Task<Result<Category>> GetCategoryAsync(Expression<Func<Category, bool>> filter = null);
        [CacheRemove("GetCategoryListAsync")]
        Task<Result> AddCategoryAsync(Category category);
        [CacheRemove("GetCategoryListAsync")]
        Task<Result> DeleteCategoryAsync(Category category);
        [CacheRemove("GetCategoryListAsync")]
        Task<Result> UpdateCategoryAsync(Category category);
        Task<Result<Category>> FindByIdAsync (int id);
    }
}