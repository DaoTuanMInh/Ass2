using Assignment2.Models;

namespace Assignment2.DTOs
{
    public class ItemDetailsDto
    {
        public List<WorkitemDetails> InforItems { get; set; } = new List<WorkitemDetails>();
        public List<ProjectDetails> Projects { get; set; } = new List<ProjectDetails>();
        public List<AssigneeDetail> Assignee { get; set; } = new List<AssigneeDetail>();
        public ICollection<Label> Labels { get; set; } = new List<Label>();
        public List<HistoryDetails> History { get; set; } = new List<HistoryDetails>();
    }
}
