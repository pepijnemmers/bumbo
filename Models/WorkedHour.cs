using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BumboApp.Models
{
    public class WorkedHour
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly DateOnly { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public HourStatus Status { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
        public List<Break>? Breaks { get; set; }
    }
}
