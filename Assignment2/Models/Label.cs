using System;
using System.Collections.Generic;

namespace Assignment2.Models;

public partial class Label
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WorkItemLabel> WorkItemLabels { get; set; } = new List<WorkItemLabel>();
}
