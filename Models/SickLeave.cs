using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BumboApp.Models
{
    public class SickLeave
    {
        public int EmployeeNumber { get; set; }
        public Employee Employee { get; set; }
        public DateOnly Date { get; set; }
    }
}
