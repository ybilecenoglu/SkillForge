using System.ComponentModel.DataAnnotations.Schema;
using CoreTestFramework.Core.Entities;

namespace CoreTestFramework.Northwind.Entities.Model;
public class Category : IEntity
{
    public int category_id { get; set; }
    
    public string category_name { get; set; }
    
    public string description { get; set; }
    
    public byte[] picture {get; set;}
    public virtual ICollection<Product> Products { get; set; }
}
