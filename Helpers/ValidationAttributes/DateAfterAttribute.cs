using System.ComponentModel.DataAnnotations;

namespace BumboApp.Helpers.ValidationAttributes
{
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly DateOnly _minDate;

        public DateAfterAttribute(int year, int month, int day)
        {
            _minDate = new DateOnly(year, month, day);
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly dateValue)
            {
                return new ValidationResult("Ongeldig datumformaat.");
            }

            if (dateValue < _minDate)
            {
                return new ValidationResult($"De datum moet na {_minDate.ToShortDateString()} zijn.");
            }

            return ValidationResult.Success!;
        }
    }
}
