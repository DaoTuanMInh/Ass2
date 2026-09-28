namespace Assignment2.DTOs
{
    public class WorkItemDto
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
    }
}
