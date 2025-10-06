using SkillForge.Core.Entities;

namespace SkillForge.Entities.Model;
public class Category : IEntity
{
    public int category_id { get; set; }
    public string category_name { get; set; }
    public string description { get; set; }
    public string picture {get; set;}
    public DateTime created_time {get; set;}
    public DateTime? modified_time {get; set;}
    public bool is_deleted {get; set;}
    public virtual ICollection<Product> Products { get; set; }
}
