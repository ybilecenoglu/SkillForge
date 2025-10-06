using SkillForge.Core.DataAccess.EntityFramework;
using SkillForge.Entities.Model;

namespace  SkillForge.DataAccess
{
    public class SupplierDAL : EntityRepositoryBase<Supplier, NorthwindContext>, ISupplierDAL
    {
        
    }
}