using CoreTestFramework.Core.Aspect.PostSharp;
using CoreTestFramework.Core.Aspect.PostSharp.Caching;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.CrossCuttingConcern.Caching.Microsoft;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.ValidationRules.FluentValidation;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.DataAccess.Concrate;
using CoreTestFramework.Northwind.Entities.Model;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CoreTestFramework.Northwind.Business.Concrate
{
    public class ProductManager : IProductService
    {
        private IProductDAL _productDal;

        public ProductManager(IProductDAL productDAL)
        {
              _productDal = productDAL;
        }
        [CacheAspect(1)]
        public async Task<Result<List<Product>>> GetProductListAsync(Expression<Func<Product, bool>> filter = null,params Expression<Func<Product, object>>[] includes)
        {
            var get_list_result = await _productDal.GetListAsync(filter, includes);
            return get_list_result;
        }
        public  Result<IQueryable<Product>> GetProductQueryable(Expression<Func<Product, bool>> filter = null,params Expression<Func<Product, object>>[] includes)
        {
            var get_queryable_result = _productDal.GetQueryable(filter, includes);
            return get_queryable_result;
        }
        public async Task<Result<Product>> GetFindByIdAsync(int id)
        {
            var product_result = await _productDal.FindByIdAsync(id);
            return product_result;
        }
        public async Task<Result<Product>> GetProductAsync(Expression<Func<Product, bool>> filter = null, params Expression<Func<Product, object>>[] includes)
        {
           var product_result = await _productDal.GetAsync(filter,includes);
           return product_result;
        }
        public async Task<Result> DeleteProductAsync(Product product)
        {
           var product_delete_result = await _productDal.UpdateAsync(product);
           return product_delete_result;
        }
        [FluentValidationAspect(typeof(ProductValidation))]
        public async Task<Result> UpdateProductAsync(Product product)
        {
            var product_update_result = await _productDal.UpdateAsync(product);
            return product_update_result;
        }
        [FluentValidationAspect(typeof(ProductValidation))]
        public async Task<Result> AddProductAsync(Product product){
            var product_create_result = await _productDal.AddAsync(product);
            return product_create_result;
        }
    }
}
