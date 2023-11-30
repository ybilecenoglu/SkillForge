using System;
using System.Collections.Generic;

namespace CoreTestFramework.Northwind.Entities.Model;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal Subtotal { get; set; }
}
