namespace Assignment2.DTOs
{
    public class WorkitemDetails
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string priority { get; set; } = null!;
        public DateTime? DueAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }



    }
}
