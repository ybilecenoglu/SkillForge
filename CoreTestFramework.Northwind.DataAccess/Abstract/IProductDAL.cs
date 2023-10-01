using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Northwind.Entities.Concrate;
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
