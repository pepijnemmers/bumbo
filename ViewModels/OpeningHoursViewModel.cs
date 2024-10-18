using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class OpeningHoursViewModel
    {
        public List<OpeningHour> OpeningHours { get; set; }
        public List<UniqueDay> UniqueDays { get; set; }
        public string[] DutchDays { get; set; } = { "Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag" };
    }
}
