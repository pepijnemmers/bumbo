
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Models

{
    [Index(nameof(Date), IsUnique = true)]

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
