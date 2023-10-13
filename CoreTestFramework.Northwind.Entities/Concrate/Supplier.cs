using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreTestFramework.Northwind.Entities.Concrate;

public partial class Supplier
{
    [Key]
    public int SupplierID { get; set; }

    public string SupplierName { get; set; }

    public string ContactName { get; set; }

    public string contact_title { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    //public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
