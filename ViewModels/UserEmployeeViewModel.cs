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

        public Employee Employee { get; set; }
    }
}
