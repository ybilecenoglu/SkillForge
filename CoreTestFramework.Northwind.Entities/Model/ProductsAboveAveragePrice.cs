using System;
using System.Collections.Generic;

namespace CoreTestFramework.Northwind.Entities.Model;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }
}
