using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
