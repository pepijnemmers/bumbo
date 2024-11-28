using System.Globalization;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        public IActionResult Index(Employee? employee, DateOnly? startDate, bool? dayView)
        {
            var role = Role.Manager; // TODO: Get role from user
            
            var selectedEmployee = employee;
            var isDayView = selectedEmployee == null ? false : dayView ?? true;
            
            var selectedStartDate = isDayView ? startDate ?? DateOnly.FromDateTime(DateTime.Today) : GetMondayOfWeek(startDate ?? DateOnly.FromDateTime(DateTime.Today));
            var weekNumber = ISOWeek.GetWeekOfYear(selectedStartDate.ToDateTime(new TimeOnly(12, 00)));
            
            var shifts = new List<Shift>(); // TODO: Get shifts from database
            var employees = Context.Employees.ToList();
                
            var viewIsConcept = shifts.Any(shift => !shift.IsFinal);
            
            var viewModel = new ScheduleViewModel()
            {
                Role = role,
                ViewIsConcept = viewIsConcept,
                SelectedStartDate = selectedStartDate,
                WeekNumber = weekNumber,
                Employees = employees,
                Shifts = shifts,
                SelectedEmployee = selectedEmployee,
                IsDayView = isDayView
            };
            return View();
        }
        
        private DateOnly GetMondayOfWeek(DateOnly date)
        {
            var dayOfWeek = (int) date.DayOfWeek;
            var daysToMonday = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            return date.AddDays(-daysToMonday);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult DeleteWeek(DateOnly startDay)
        {
            return RedirectToAction("Index");
        }
    }
}
