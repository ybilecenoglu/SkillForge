using System.Linq.Expressions;
using SkillForge.Core;
using SkillForge.Core.Common;
using SkillForge.Entities.Model;

namespace SkillForge.Business
{
    public interface ISupplierService
    {
        Task<Result<List<Supplier>>> GetSupplierListAsync(Expression<Func<Supplier, bool>> expression);
    }
}