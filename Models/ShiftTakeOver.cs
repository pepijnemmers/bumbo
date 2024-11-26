using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BumboApp.Models
{
    public class ShiftTakeOver
    {
        [Key]
        public int ShiftId { get; set; }
        [Required]
        [ForeignKey(nameof(ShiftId))]
        public Shift Shift { get; set; }
        [ForeignKey(nameof(ShiftId))]
        public int? EmployeeTakingOverEmployeeNumber { get; set; }
        public Employee? EmployeeTakingOver { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
