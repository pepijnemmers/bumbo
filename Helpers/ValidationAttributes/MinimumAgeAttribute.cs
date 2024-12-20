using System.ComponentModel.DataAnnotations;

namespace BumboApp.Helpers.ValidationAttributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinimumAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly dateValue)
            {
                return new ValidationResult("Ongeldig datumformaat.");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var thresholdDate = today.AddYears(-_minAge);

            if (dateValue > thresholdDate)
            {
                return new ValidationResult($"De datum moet voor {thresholdDate.ToShortDateString()} zijn (minimaal {_minAge} jaar oud).");
            }

            return ValidationResult.Success!;
        }
    }
}
