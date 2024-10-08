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
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        [StringLength(45)]
        public string Password { get; set; }
    }
}
