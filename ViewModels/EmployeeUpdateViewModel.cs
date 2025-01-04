using BumboApp.Helpers.ValidationAttributes;
using BumboApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class EmployeeUpdateViewModel
    {
        public int EmployeeNumber { get; set; }

        [DutchRequired(ErrorMessage = "e-mail")]
        [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9._-]*[a-zA-Z0-9])?@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})*$",
    ErrorMessage = "Vul een geldig email adres in")]
        public string Email { get; set; }

        [DutchRequired(ErrorMessage = "functie")]
        public Role Role { get; set; }

        [DutchRequired(ErrorMessage = "voornaam")]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters")]
        public string FirstName { get; set; }

        [DutchRequired(ErrorMessage = "achternaam")]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alleen letters")]
        public string LastName { get; set; }

        [DateAfter(1900, 1, 1)]
        [MinimumAge(15)]
        [DutchRequired(ErrorMessage = "geboortedatum")]
        public DateOnly DateOfBirth { get; set; }

        [DutchRequired(ErrorMessage = "postcode")]
        [StringLength(6)]
        [RegularExpression(@"^[1-9]\d{3}[a-zA-Z]{2}$", ErrorMessage = "Postcode moet van het formaat '1234AB' zijn")]
        public string Zipcode { get; set; }

        [DutchRequired(ErrorMessage = "huisnummer")]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]+[A-Za-z]*$", ErrorMessage = "Huisnummer moet beginnen met een cijfer en (optioneel) eindigen met letters")]
        public string HouseNumber { get; set; }

        [DutchRequired(ErrorMessage = "contracturen"), Range(0, 55, ErrorMessage = "De waarde van contracturen moet minimaal 0 en maximaal 55 zijn.")]
        public int ContractHours { get; set; }

        public DateOnly StartOfEmployment { get; set; }

        public DateOnly? EndOfEmployment { get; set; }

        //Voor niet editen van je eigen rol, om te behouden na !Modelstate.IsValid (ViewData gaat hierbij verloren)
        public bool IsOwner { get; set; }
    }
}
