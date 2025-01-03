using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Break
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        // Navigation properties
        public WorkedHour WorkedHour { get; set; }
    }
}
