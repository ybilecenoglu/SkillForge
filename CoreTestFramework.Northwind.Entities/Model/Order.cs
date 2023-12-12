using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace CoreTestFramework.Northwind.Entities.Model;

public partial class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }
    [Key]
    public int OrderID { get; set; }

    public string CustomerID { get; set; }

    public int EmployeeID { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public DateTime ShippedDate { get; set; }
    [ForeignKey(typeof(Shipper))]
    public int ShipVia { get; set; }

    public decimal Freight { get; set; }

    public string ShipName { get; set; }

    public string ShipAddress { get; set; }

    public string ShipCity { get; set; }

    public string ShipRegion { get; set; }

    public string ShipPostalCode { get; set; }

    public string ShipCountry { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Employee Employee { get; set; }
    
    [OneToMany]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Shipper Shippers { get; set; }
}
