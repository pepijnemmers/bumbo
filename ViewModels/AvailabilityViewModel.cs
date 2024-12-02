using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class AvailabilityViewModel
    {
        public int WeekNr { get; set; }
        public DateOnly StartDate { get; set; }
        public List<Availability> AvailabilityList { get; set; } = new List<Availability>();
        public List<SchoolSchedule> SchoolScheduleList { get; set; } = new List<SchoolSchedule>();
    }
}
