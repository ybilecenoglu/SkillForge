using SkillForge.DataAccess.Abstract;
using SkillForge.Entities.Model;
using SkillForge.Core.DataAccess.EntityFramework;

namespace SkillForge.DataAccess.Concrate
{
    public class ProductDAL: EntityRepositoryBase<Product, NorthwindContext>, IProductDAL
    {

    }
}
