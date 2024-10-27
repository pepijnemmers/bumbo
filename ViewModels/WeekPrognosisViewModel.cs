using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class WeekPrognosisViewModel
    {
        public DateOnly StartDate { get; set; }
        public DateOnly CurrentDate {  get; set; }
        public int WeekNr { get; set; }
        public int Year { get; set; }
        public List<Prognosis> Prognoses { get; set; }
        public string[] DutchDays { get; set; } = { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag" };
    }
}
