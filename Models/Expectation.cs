
using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models

{
    public class Expectation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public int ExpectedCustomers { get; set; }

        [Required]
        public int ExpectedCargo { get; set; }
    }
}
