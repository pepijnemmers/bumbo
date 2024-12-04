using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class StandardAvailability
    {
        [Required]
        public int EmployeeNumber { get; set; }
        [Required]
        public DayOfWeek Day { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }

        //Navigational property
        public Employee? Employee { get; set; }
    }
}
