namespace Assignment2.Models
{
    public class WorkItemLabel
    {
        public long WorkItemId { get; set; }
        public long LabelId { get; set; }

        public WorkItem workItem { get; set; }
        public Label label { get; set; }
    }
}
