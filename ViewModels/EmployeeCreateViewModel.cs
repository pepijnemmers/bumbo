using BumboApp.Helpers.ValidationAttributes;
using BumboApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.ViewModels
{
    public class EmployeeCreateViewModel : EmployeeUpdateViewModel
    {
        [DutchRequired(ErrorMessage = "wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
