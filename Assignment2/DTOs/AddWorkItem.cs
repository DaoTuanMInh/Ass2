
using System.ComponentModel.DataAnnotations;

namespace Assignment2.DTOs
{
    public class AddWorkItem
    {
        [StringLength(100, MinimumLength =5)]
        public string Title { get; set; } = null!;
        [StringLength(2000, MinimumLength = 0)]
        public string? Description { get; set; }
        public string ProjectCode { get; set; } = null!;
        public long? AssigneeId { get; set; }
        public string Priority { get; set; } = null!;
        public DateTime? DueAt { get; set; }
        public virtual string[] Labels { get; set; } 
    }
}
