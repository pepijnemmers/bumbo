using System.ComponentModel.DataAnnotations;

namespace BumboApp.Models
{
    public class UniqueDay
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required, Range(0, float.MaxValue)]
        public float Factor { get; set; }
    }
}
