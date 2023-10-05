using CoreTestFramework.Core.DataAccess;
using CoreTestFramework.Core.DataAccess.EntityFramework;
using CoreTestFramework.Core.DataAccess.Nhibarnate;
using CoreTestFramework.Northwind.DataAccess.Abstract;
using CoreTestFramework.Northwind.DataAccess.ORM;
using CoreTestFramework.Northwind.Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Northwind.DataAccess.Concrate
{
    public class ProductDAL: EntityRepositoryBase<Product, NorthwindContext>, IProductDAL
    {

    }
}
