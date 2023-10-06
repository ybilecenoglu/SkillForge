using CoreTestFramework.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Product : IEntity
{
    [Key]
    public int product_id { get; set; }

    public string product_name { get; set; } = null!;

    public int? supplier_id { get; set; }

    public int? category_id { get; set; }

    public string quantity_per_unit { get; set; }

    public double? unit_price { get; set; }

    public short? units_in_stock { get; set; }

    public short? units_on_order { get; set; }

    public short? reorder_level { get; set; }

    public int discontinued { get; set; }
    public string product_image {get; set;}

    public string categoryName()
    {
        using (var db = new NorthwindContext())
        {
            var category_result = db.Categories.Where(c => c.category_id == category_id).FirstOrDefault();
            return category_result.category_name;
        }
    }
    public string supplierName()
    {
        using (var db = new NorthwindContext())
        {
            var supplier_result = db.Suppliers.Where(s => s.supplier_id == supplier_id).FirstOrDefault();
            return supplier_result.company_name;
        }
    }

    //public virtual Category? Category { get; set; }

    //public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    //public virtual Supplier? Supplier { get; set; }
}
