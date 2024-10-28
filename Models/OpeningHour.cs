using System.ComponentModel.DataAnnotations;
namespace BumboApp.Models
{
    public class OpeningHour
    {
        [Key]
        public DayOfWeek WeekDay { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
    }
}
