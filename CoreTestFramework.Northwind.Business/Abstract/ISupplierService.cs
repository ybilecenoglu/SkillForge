using System.Linq.Expressions;
using CoreTestFramework.Core;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Business
{
    public interface ISupplierService
    {
        Task<Result<List<Supplier>>> GetSupplierListAsync(Expression<Func<Supplier, bool>> expression);
    }
}