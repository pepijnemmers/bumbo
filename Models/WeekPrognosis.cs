using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models

{
    public class WeekPrognosis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public List<Prognosis> Prognoses { get; set; }
    }
}
