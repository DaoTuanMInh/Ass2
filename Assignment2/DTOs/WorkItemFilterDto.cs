namespace Assignment2.DTOs
{
    public class WorkItemFilterDto
    {
        public string? keyword { get; set; }
        public string? status { get; set; }
        public string? priority { get; set; }
        public string? projectCode { get; set; }
        public int? assigneeId { get; set; } 
        public bool overdue { get; set; } = false; 
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 20; 
        public string? sort { get; set; } = "-createdAt";
    }
}
