using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Northwind.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Northwind.DataAccess.Abstract
{
    public interface IProductDAL : IEntityRepository<Product>
    {

    }
}
