using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Assignment2.DTOs
{
    public class AssigneeItem
    {
        public long AssigneeId { get; set; }
        [MaxLength(1000, ErrorMessage = "Note tối đa 1.000 ký tự.")]
        public String Note { get; set; } = null!;
    }
}
