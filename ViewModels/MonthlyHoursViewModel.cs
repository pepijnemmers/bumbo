using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class MonthlyHoursViewModel
    {
        public required int SelectedMonth { get; set; }
        public required int SelectedYear { get; set; }
        public required double WorkedHours { get; set; }
        public required double PlannedHours { get; set; }
        public required double Difference {  get; set; }

        public string[] Months = ["Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli", "Augustus", "September", "Oktober", "November", "December"];
    }
}
