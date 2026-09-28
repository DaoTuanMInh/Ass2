using System;
using System.Collections.Generic;

namespace Assignment2.Models;

public partial class WorkItem
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public long ProjectId { get; set; }

    public long? AssigneeId { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Developer? Assignee { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<WorkItemHistory> WorkItemHistories { get; set; } = new List<WorkItemHistory>();

    public virtual ICollection<WorkItemLabel> WorkItemLabels { get; set; } = new List<WorkItemLabel>();
}
