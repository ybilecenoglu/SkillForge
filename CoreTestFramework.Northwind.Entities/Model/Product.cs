using System.ComponentModel.DataAnnotations.Schema;
using CoreTestFramework.Core.Entities;

namespace CoreTestFramework.Northwind.Entities.Model;
public class Product : IEntity
{
    public int ProductID { get; set; }
    public string ProductName { get; set;}
    public int SupplierId { get; set; }
    public int CategoryId { get; set; }
    public string QuantityPerUnit { get; set; }
    public double UnitPrice { get; set; }
    public int UnitsInStock {get; set;}
    public int UnitsOnOrder {get; set;}
    public int ReorderLevel {get; set;}
    public string Discontinued {get; set;}
    public virtual Supplier Supplier {get; set;}
    public virtual Category Category {get; set;}
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
