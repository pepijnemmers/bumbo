using BumboApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class UserEmployeeViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }


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
        [RegularExpression(@"^\d{4}[A-Z]{2}$", ErrorMessage = "Postcode moet van het formaat '1234AB'")]
        public string Zipcode { get; set; }
        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }
        [Required, Range(0, 55)]
        public int ContractHours { get; set; }
        public int LeaveHours { get; set; }

        //public Employee Employee { get; set; }
    }
}
