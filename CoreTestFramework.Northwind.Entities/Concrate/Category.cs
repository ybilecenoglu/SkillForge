using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Category
{
    [Key]
    public int category_id { get; set; }

    public string category_name { get; set; } = null!;

    public string description { get; set; }

    public byte[] picture { get; set; }

    //public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
