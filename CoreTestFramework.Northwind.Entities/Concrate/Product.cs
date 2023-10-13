using CoreTestFramework.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Product : IEntity
{
    [Key]
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!;
    public int? SupplierID { get; set; }
    public int? CategoryID { get; set; }
    public string Unit { get; set; }
    public double? Price { get; set; }

    public string categoryName()
    {
        using (var db = new NorthwindContext())
        {
            var category_result = db.Categories.Where(c => c.CategoryID == CategoryID).FirstOrDefault();
            return category_result.CategoryName;
        }
    }
    public string supplierName()
    {
        using (var db = new NorthwindContext())
        {
            var supplier_result = db.Suppliers.Where(s => s.SupplierID == SupplierID).FirstOrDefault();
            return supplier_result.SupplierName;
        }
    }
    // public virtual Category Category { get; set; }
    // public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    // public virtual Supplier Supplier { get; set; }
}
