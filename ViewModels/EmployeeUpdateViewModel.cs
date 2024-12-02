using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class EmployeeUpdateViewModel
    {
        public int EmployeeNumber { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [StringLength(6)]
        public string Zipcode { get; set; }
        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }
        [Required, Range(0, 55)]
        public int ContractHours { get; set; }
        [Required, Range(0, 255)]
        public int LeaveHours { get; set; }
        [Required]
        public DateOnly StartOfEmployment { get; set; }
        public DateOnly? EndOfEmployment { get; set; }
        public string Email { get; set; }
    }
}
