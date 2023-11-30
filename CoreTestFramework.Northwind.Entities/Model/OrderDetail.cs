using Microsoft.EntityFrameworkCore;
using SQLiteNetExtensions.Attributes;

namespace CoreTestFramework.Northwind.Entities.Model;

// [Keyless] //Primary Key olmayan entitiylerde "HasNoKey" fonksiyonuna alternatif olarak kullanildi
public partial class OrderDetail
{
    [ForeignKey(typeof(Order))]
    public int OrderID { get; set; }
    [ForeignKey(typeof(Product))]
    public int? ProductID { get; set; }

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

    [ManyToOne]
    public virtual Order Order { get; set; } 

    [ManyToOne]
    public Product Product { get; set; }
}
