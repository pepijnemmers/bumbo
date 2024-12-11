using BumboApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class EmployeeUpdateViewModel
    {
        public int EmployeeNumber { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters")]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [StringLength(6)]
        [RegularExpression(@"^\d{4}[A-Z]{2}$", ErrorMessage = "Postcode moet van het formaat '1234AB'")]
        public string Zipcode { get; set; }
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]+[A-Za-z]*$", ErrorMessage = "Huisnummer moet beginnen met een cijfer en (optioneel) eindigen met letters")]
        public string HouseNumber { get; set; }
        [Required, Range(0, 55)]
        public int ContractHours { get; set; }
        [Required, Range(0, 120)]
        public int LeaveHours { get; set; }
        [Required]
        public DateOnly StartOfEmployment { get; set; }
        public DateOnly? EndOfEmployment { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
            ErrorMessage = "Vul een geldig email adres in")]
        public string Email { get; set; }
        public Role Role { get; set; }

        //Voor niet editen van je eigen rol, om te behouden na !Modelstate.IsValid (ViewData gaat hierbij verloren)
        public bool IsOwner { get; set; }
    }
}
