using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models
{
    public class OpeningHour
    {
        [Key]
        [StringLength(10)]
        public string WeekDay { get; set; } = null!;
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
    }
}
