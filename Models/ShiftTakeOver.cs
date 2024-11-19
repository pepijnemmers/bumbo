using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class ShiftTakeOver
    {
        [Key]
        public int ShiftId { get; set; }
        [Required]
        public Shift Shift { get; set; }
        public Employee EmployeeTakingOver { get; set; }
    }
}
