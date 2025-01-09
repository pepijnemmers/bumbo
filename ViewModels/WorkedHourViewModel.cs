using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.ViewModels
{
    public class WorkedHourViewModel
    {
        public Employee Employee { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string Breaks { get; set; }
        public string Status { get; set; }
        public Shift PlannedShift { get; set; }
        public bool IsFuture { get; set; }
        public required DateOnly SelectedStartDate { get; set; }
    }
}
