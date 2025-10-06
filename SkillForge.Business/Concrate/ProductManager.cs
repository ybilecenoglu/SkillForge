using SkillForge.Core.Common;
using SkillForge.DataAccess.Abstract;
using SkillForge.Entities.Model;
using System.Linq.Expressions;

namespace SkillForge.Business.Concrate
{
    public class ProductManager : IProductService
    {
        private IProductDAL _productDal;

        public ProductManager(IProductDAL productDAL)
        {
              _productDal = productDAL;
        }
        
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
        public async Task<Result> UpdateProductAsync(Product product)
        {
            var product_update_result = await _productDal.UpdateAsync(product);
            return product_update_result;
        }
        
        public async Task<Result> AddProductAsync(Product product)
        {
            var product_create_result = await _productDal.AddAsync(product);
            return product_create_result;
        }
    }
}
