using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.ViewModels
{
    public class WorkedHourViewModel
    {
        public int? Id { get; set; }
        public Employee? Employee { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public TimeSpan? BreaksDuration { get; set; }
        public TimeSpan? TotalWorkedTime { get; set; }
        public HourStatus? Status { get; set; }
        public string? PlannedShift { get; set; }
        public bool IsFuture { get; set; }

        public bool HasHourDifference { get; set; }
    }
}
