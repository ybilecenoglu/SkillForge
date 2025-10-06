using System;
using System.Collections.Generic;

namespace SkillForge.Entities.Model;

public partial class Territory
{
    public string TerritoryID { get; set; } = null!;

    public string TerritoryDescription { get; set; } = null!;

    public int RegionId { get; set; }

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
