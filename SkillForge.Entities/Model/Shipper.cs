using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Entities.Model;

public partial class Shipper
{
    
    public int shipper_id { get; set; }
    
    public string company_name { get; set; } = null!;
    
    public string phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
