using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models
{
    public class Prognosis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly Date {  get; set; }
        [Required]
        [StringLength(255)]
        public string Department { get; set; }
        [Required]
        public float NeededHours { get; set; }
        [Required]
        public float NeededEmployees { get; set; }
    }
}
