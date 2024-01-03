using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.DataAccess
{
    public interface ICategoryDAL : IEntityRepository<Category>
    {

    }
}