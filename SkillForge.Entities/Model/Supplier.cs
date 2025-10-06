
using SkillForge.Core.Entities;
namespace SkillForge.Entities.Model;
public class Supplier : IEntity
{
    
    public int supplier_id { get; set; }
    
    public string company_name { get; set; }
    
    public string contact_name { get; set; }
    
    public string contact_title { get; set; }
    
    public string address { get; set; }
    
    public string city { get; set; }
    
    public string region {get; set;}
    
    public string postal_code { get; set; }
    
    public string country { get; set; }
    
    public string phone { get; set; }
    
    public string fax {get; set;}
    
    public string homepage{get; set;}
    public virtual ICollection<Product> Products { get; set; }
}
