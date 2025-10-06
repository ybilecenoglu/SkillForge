using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Entities.Model;

public partial class Customer
{
    public string customer_id { get; set; }
    public string company_name { get; set; }
    public string contact_name { get; set; }
    public string contact_title { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string region { get; set; }
    public string postal_code { get; set; }
    public string country { get; set; }
    public string phone { get; set; }
    public string fax { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();
}
