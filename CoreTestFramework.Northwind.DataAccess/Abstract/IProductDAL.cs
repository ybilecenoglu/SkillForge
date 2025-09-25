using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.DataAccess.Abstract
{
    public interface IProductDAL : IEntityRepository<Product>
    {

    }
}
