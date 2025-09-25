using CoreTestFramework.Core;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.Entities.ValidationRules.FluentValidation;
using System.Linq.Expressions;

namespace CoreTestFramework.Northwind.Business
{
    public interface IProductService
    {
        [Cache]
        Task<Result<List<Product>>> GetProductListAsync(Expression<Func<Product, bool>> filter = null, params Expression<Func<Product, object>>[] includes);
        Result<IQueryable<Product>> GetProductQueryable(Expression<Func<Product, bool>> filter = null, params Expression<Func<Product, object>>[] includes);
        Task<Result<Product>> GetFindByIdAsync(int id);
        Task<Result<Product>> GetProductAsync(Expression<Func<Product, bool>> filter = null,params Expression<Func<Product, object>>[] includes);
        Task<Result> DeleteProductAsync(Product product);
        Task<Result> UpdateProductAsync(Product product);
        [FluentValidation(typeof(ProductValidation))]
        [CacheRemove("GetProductListAsync")]
        Task<Result> AddProductAsync(Product product);
    }
}
