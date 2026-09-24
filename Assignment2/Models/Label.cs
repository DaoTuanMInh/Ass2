using System;
using System.Collections.Generic;

namespace Assignment2.Models;

public partial class Label
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}
