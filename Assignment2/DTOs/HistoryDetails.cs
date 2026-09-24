namespace Assignment2.DTOs
{
    public class HistoryDetails
    {
        public string? FromStatus { get; set; }

        public string? ToStatus { get; set; }

        public string? Note { get; set; }

        public string ChangedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
