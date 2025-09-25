using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.DataAccess
{
    public interface ISupplierDAL : IEntityRepository<Supplier>
    {
        
    }
}