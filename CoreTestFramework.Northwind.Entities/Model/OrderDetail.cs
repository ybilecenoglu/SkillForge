using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreTestFramework.Northwind.Entities.Model;


public partial class OrderDetail
{
   
    public int order_id { get; set; }
    
    public int product_id { get; set; }
    
    public double unit_price { get; set; }
    
    public short quantity { get; set; }

    public double discount { get; set; }
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}
