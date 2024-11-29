using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class AvailabilityViewModel
    {
        public int WeekNr { get; set; }
        public DateOnly StartDate { get; set; }
        public List<Availability> availabilityList { get; set; } = new List<Availability>();
        public List<SchoolSchedule> schoolScheduleList { get; set; } = new List<SchoolSchedule>();
    }
}
