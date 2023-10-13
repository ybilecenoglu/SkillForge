using CoreTestFramework.Core.DataAccess.EntityFramework;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.Entities.Concrate;

namespace CoreTestFramework.Northwind.DataAccess.Concrate
{
    public class ProductDAL: EntityRepositoryBase<Product, NorthwindContext>, IProductDAL
    {

    }
}
