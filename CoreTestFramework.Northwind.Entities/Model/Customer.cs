using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SQLiteNetExtensions.Attributes;

namespace CoreTestFramework.Northwind.Entities.Model;

public partial class Customer
{
    [Key]
    public string CustomerID { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string ContactName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string Fax { get; set; }

    [OneToMany]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();
}
