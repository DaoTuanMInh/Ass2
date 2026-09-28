using System.Diagnostics;

namespace Assignment2.Common
{
    public class respone<T>
    {
        public string TraceId { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
