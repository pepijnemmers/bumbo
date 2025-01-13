using BumboApp.Models;

namespace BumboApp.ViewModels
{
    public class DifferingMonthlyHoursViewModel
    {
        public required int SelectedMonth { get; set; }
        public required int SelectedYear { get; set; }
        public Employee? Employee { get; set; }
        public List<WorkedHourViewModel> WorkedHours { get; set; } = new List<WorkedHourViewModel>();

        public string[] Months = ["Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli", "Augustus", "September", "Oktober", "November", "December"];
    }
}
