using BumboApp.Models;
using System.Globalization;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        public IActionResult Index(string? employeeNumber, DateOnly? startDate, bool dayView = true)
        {
            var role = LoggedInUserRole;
            
            var selectedEmployee = employeeNumber != null && int.TryParse(employeeNumber, out _) ? Context.Employees.Find(int.Parse(employeeNumber)) : null;
            var isDayView = selectedEmployee == null || dayView;
            
            var selectedStartDate = isDayView ? startDate ?? DateOnly.FromDateTime(DateTime.Today) : GetMondayOfWeek(startDate ?? DateOnly.FromDateTime(DateTime.Today));
            var weekNumber = ISOWeek.GetWeekOfYear(selectedStartDate.ToDateTime(new TimeOnly(12, 00)));
            
            var employees = Context.Employees.ToList();
            var shifts = new List<Shift>(); // TODO: Get shifts from database > for day view of all employees, order by department

            if (isDayView)
            {
                if (selectedEmployee != null)
                {
                    shifts = Context.Shifts
                        .Where(shift => shift.Employee == selectedEmployee 
                                        && DateOnly.FromDateTime(shift.Start) == selectedStartDate)
                        .OrderBy(shift => shift.Department)
                        .ToList();
                }
                else
                {
                    shifts = Context.Shifts
                        .Where(shift => DateOnly.FromDateTime(shift.Start) == selectedStartDate)
                        .OrderBy(shift => shift.Department)
                        .ToList();
                }
            }
            else
            {
                
            }
            
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
            return View(viewModel);
        }
        
        private DateOnly GetMondayOfWeek(DateOnly date)
        {
            var dayOfWeek = (int) date.DayOfWeek;
            var daysToMonday = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            return date.AddDays(-daysToMonday);
        }

        public IActionResult Create()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult DeleteWeek(DateOnly startDay)
        {
            return RedirectToAction("Index");
        }
    }
}
