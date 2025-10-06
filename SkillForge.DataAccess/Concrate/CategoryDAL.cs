using SkillForge.Core.DataAccess.EntityFramework;
using SkillForge.Entities.Model;

namespace SkillForge.DataAccess
{
    public class CategoryDAL : EntityRepositoryBase<Category, NorthwindContext>,ICategoryDAL 
    {
        
    }
}