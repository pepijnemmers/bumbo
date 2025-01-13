using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(255)]
        public string Reason { get; set; }
        [Required]
        public Employee? Employee { get; set; }
    }
}
