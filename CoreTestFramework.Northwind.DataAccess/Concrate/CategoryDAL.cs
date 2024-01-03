using CoreTestFramework.Core.DataAccess.EntityFramework;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.DataAccess
{
    public class CategoryDAL : EntityRepositoryBase<Category, NorthwindContext>,ICategoryDAL 
    {
        
    }
}