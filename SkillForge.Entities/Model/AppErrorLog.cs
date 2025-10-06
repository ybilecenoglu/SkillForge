using System;
using System.Collections.Generic;

namespace SkillForge.Entities.Model;

public partial class AppErrorLog
{
    public int Id { get; set; }

    public string Message { get; set; }

    public string Action { get; set; }

    public string Target { get; set; }

    public DateTime Date { get; set; }
}
