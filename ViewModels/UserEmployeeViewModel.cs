using BumboApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class UserEmployeeViewModel
    {
        [Required(ErrorMessage = "Waarom werkt dit niet?")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }


        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
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
        [Required, Range(0, 120)] //TODO dit veld moet weg, berekenen van contracturen
        public int LeaveHours { get; set; }
    }
}
