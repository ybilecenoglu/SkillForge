using System.Linq.Expressions;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Business
{
    public class SupplierManager : ISupplierService
    {
        private readonly ISupplierDAL _supplierDAL;
        public SupplierManager(ISupplierDAL supplierDAL)
        {
            _supplierDAL = supplierDAL;
        }
        public async Task<Result<List<Supplier>>> GetSupplierListAsync(Expression<Func<Supplier, bool>> expression)
        {
            var supplier_result = await _supplierDAL.GetListAsync(expression);
            return supplier_result;

        }
    }
}