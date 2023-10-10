using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Entities.Concrate;
using System.Linq.Expressions;

namespace CoreTestFramework.Northwind.Business.Abstract
{
    public interface IProductService
    {
        Task<Result<List<Product>>> GetProductListAsync(Expression<Func<Product, bool>> filter = null);
        Task<Result<Product>> GetProductAsync(int id);
        Result<IQueryable<Product>> GetProductQueryable(Expression<Func<Product, bool>> filter = null);
        Task<Result> DeleteProductAsync(Product product);
    }
}
