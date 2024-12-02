using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Availability
    {
        public DateOnly Date { get; set; }
        [Required]
        public int EmployeeNumber { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
