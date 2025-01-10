using Microsoft.AspNetCore.Mvc;

namespace BumboApp.ViewModels
{
    public class WorkedHoursPageViewModel
    {
        // Filters
        public DateOnly SelectedStartDate { get; set; }
        public string? Employee { get; set; }
        public string? Hours { get; set; }

        // List of Worked Hours
        public List<WorkedHourViewModel> WorkedHours { get; set; } = new List<WorkedHourViewModel>();
    }
}
