using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public Department Department { get; set; }
        [Required]
        public Employee Employee { get; set; }
        //Navigation
        public ShiftTakeOver ShiftTakeOver { get; set; }
    }
}
