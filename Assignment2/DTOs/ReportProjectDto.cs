namespace Assignment2.DTOs
{
    public class ReportProjectDto
    {
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public int TotalItems { get; set; }
        public int OpenItems { get; set; }
        public int OverdueItems { get; set; }
        public int DoneItems { get; set; }
        public double AverageCompletionHours { get; set; }
    }
}
