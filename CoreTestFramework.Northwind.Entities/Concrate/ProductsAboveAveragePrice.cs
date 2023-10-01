using System;
using System.Collections.Generic;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
