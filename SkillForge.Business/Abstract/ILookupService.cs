using System.Linq.Expressions;
using SkillForge.Core;
using SkillForge.Core.Common;
using SkillForge.Entities.Model;

namespace SkillForge.Business
{
    public interface ILookupService
    {
        Task<Result<List<Product>>> GetLookupProductsAsync(Expression<Func<Product, bool>> expression);
        [Cache(60*24)]
        Task<Result<List<Category>>> GetLookupCategoriesAsync(Expression<Func<Category, bool>> expression);
        [Cache(60*24)]
        Task<Result<List<Supplier>>> GetLookupSuppliersAsync(Expression<Func<Supplier, bool>> expression);
    }
}
