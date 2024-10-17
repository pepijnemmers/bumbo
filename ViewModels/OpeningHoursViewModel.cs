using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class OpeningHoursViewModel
    {
        public List<OpeningHour> OpeningHours { get; set; }
        public List<UniqueDay> UniqueDays { get; set; }
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        //public int MaxPages { get; set; }
        //public string ImageUrl { get; set; }
        //public bool OverviewDesc { get; set; }
        //public char? UsePassedDates { get; set; }


        public string[] DutchDays { get; set; } = { "Zondag","Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag" };
    }
}
