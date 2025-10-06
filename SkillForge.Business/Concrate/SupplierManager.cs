using System.Linq.Expressions;
using SkillForge.Core.Common;
using SkillForge.DataAccess;
using SkillForge.Entities.Model;

namespace SkillForge.Business
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