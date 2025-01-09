using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.ViewModels
{
    public class WorkedHourViewModel
    {
        public Employee? Employee { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        //public List<string> Breaks { get; set; }
        public TimeSpan? BreaksDuration { get; set; }
        public string Status { get; set; }
        public Shift PlannedShift { get; set; }
        public bool IsFuture { get; set; }
    }
}
