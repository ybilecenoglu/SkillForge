using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Category
{
    [Key]
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public byte Picture {get; set;}
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
