using CoreTestFramework.Core.DataAccess.EntityFramework;
using CoreTestFramework.Northwind.Entities.Model;

namespace  CoreTestFramework.Northwind.DataAccess
{
    public class SupplierDAL : EntityRepositoryBase<Supplier, NorthwindContext>, ISupplierDAL
    {
        
    }
}