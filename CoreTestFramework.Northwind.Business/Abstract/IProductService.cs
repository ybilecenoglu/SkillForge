using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Northwind.Business.Abstract
{
    public interface IProductService
    {
        Task<Result<List<Product>>> GetProductListAsync(Expression<Func<Product, bool>> filter = null);
        Result<IQueryable<Product>> GetProductQueryable(Expression<Func<Product, bool>> filter = null);
    }
}
