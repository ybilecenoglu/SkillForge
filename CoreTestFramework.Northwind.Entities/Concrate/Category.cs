using System;
using System.Collections.Generic;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Category
{
    public int category_id { get; set; }

    public string category_name { get; set; } = null!;

    public string? description { get; set; }

    public byte[]? picture { get; set; }

    //public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
