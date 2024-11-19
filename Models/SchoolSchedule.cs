using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class SchoolSchedule
    {
        public int EmployeeNumber { get; set; }
        public Employee Employee { get; set; }
        public DateOnly Date { get; set; }
        [Required]
        public float DurationInHours { get; set; }
    }
}
