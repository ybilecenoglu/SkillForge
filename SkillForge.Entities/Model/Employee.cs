using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Entities.Model;

public partial class Employee
{
    public int employee_id { get; set; }
    public string last_name { get; set; } 
    public string first_name { get; set; }
    public string title { get; set; }
    public string title_of_courtesy { get; set; }
    public DateTime birth_date { get; set; }
    public DateTime hire_date { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string region { get; set; }
    public string postal_code { get; set; }
    public string country { get; set; }
    public string home_phone { get; set; }
    public string extension { get; set; }
    public byte[] photo { get; set; }
    public string notes { get; set; }
    public int? reports_to { get; set; }
    public string photo_path { get; set; }

    public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    [ForeignKey("employee_id")]
    public virtual Employee ReportsToNavigation { get; set; }
    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();
}
