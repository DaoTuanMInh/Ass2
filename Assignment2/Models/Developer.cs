using System;
using System.Collections.Generic;

namespace Assignment2.Models;

public partial class Developer
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Team { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}
