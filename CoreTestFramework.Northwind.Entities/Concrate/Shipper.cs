using System;
using System.Collections.Generic;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Shipper
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
