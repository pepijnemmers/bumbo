using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models
{
    public class Norm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Value { get; set; } //TODO Was called Norm in KD as well, any alternative names?
        [Required]
        public NormType NormType { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
