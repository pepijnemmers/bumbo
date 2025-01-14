using System.ComponentModel.DataAnnotations;

namespace BumboApp.Helpers.ValidationAttributes
{
    public class DutchRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string attributeName)
        {
            string fieldname = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : attributeName;
            return $"Het veld {fieldname} is vereist.";
        }
    }
}
