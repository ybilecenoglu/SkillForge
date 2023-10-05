using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Concrate;
using System.Linq.Expressions;

namespace CoreTestFramework.Northwind.Business.Concrate
{
    public class ProductManager : IProductService
    {
        private ProductDAL _productDal;

        public ProductManager(ProductDAL productDAL)
        {
              _productDal = productDAL;
        }
        public async Task<Result<List<Product>>> GetProductListAsync(Expression<Func<Product, bool>> filter = null)
        {
            var get_list_result = await _productDal.GetListAsync(filter);
            return get_list_result;
            
        }

        public Result<IQueryable<Product>> GetProductQueryable(Expression<Func<Product, bool>> filter = null)
        {
            var get_queryable_result = _productDal.GetQueryable(filter);
            return get_queryable_result;
        }
    }
}
