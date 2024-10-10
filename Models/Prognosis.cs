using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models
{
    public class Prognosis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WeekPrognosesId { get; set; }
        [Required]
        public DateOnly Date {  get; set; }
        [Required]
        public Department Department { get; set; }
        [Required]
        public float NeededHours { get; set; }
        [Required]
        public float NeededEmployees { get; set; }
    }
}
