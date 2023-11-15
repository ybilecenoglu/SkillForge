using CoreTestFramework.Core.Aspect.PostSharp;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business.Abstract;
using CoreTestFramework.Northwind.Business.ValidationRules.FluentValidation;
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
        
        public async Task<Result<Product>> GetProductAsync(int id)
        {
            var product_result = await _productDal.FindById(id);
            return product_result;
        }

        public async Task<Result> DeleteProductAsync(Product product)
        {
           var product_delete_result = await _productDal.DeleteAsync(product);
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
