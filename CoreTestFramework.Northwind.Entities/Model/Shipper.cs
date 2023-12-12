using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SQLiteNetExtensions.Attributes;

namespace CoreTestFramework.Northwind.Entities.Model;

public partial class Shipper
{
    [Key]
    public int ShipperID { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Phone { get; set; }

    [OneToMany]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
