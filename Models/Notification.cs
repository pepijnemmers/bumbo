using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Required, StringLength(60)]
        public string Title { get; set; }
        [Required, StringLength(60)]
        public string Description { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        [Required]
        public bool HasBeenRead { get; set; }
        [Column(TypeName = "text")]
        public string? ActionUrl { get; set; }
    }
}
