using CoreTestFramework.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Product : IEntity
{
    [Key]
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int SupplierID { get; set; }
    public virtual Supplier Supplier {get; set;}
    public int CategoryID { get; set; }
    public virtual Category Category {get; set;}
    public string QuantityPerUnit { get; set; }
    public double UnitPrice { get; set; }
    public int UnitsInStock {get; set;}
    public int UnitsOnOrder {get; set;}
    public int ReorderLevel {get; set;}
    public string Discontinued {get; set;}

    // public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    
}
