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
            var role = Role.Manager; // TODO: Get role from user
            
            var selectedEmployee = employeeNumber != null && int.TryParse(employeeNumber, out _) ? Context.Employees.Find(int.Parse(employeeNumber)) : null;
            var isDayView = selectedEmployee == null || dayView;
            
            var selectedStartDate = isDayView ? startDate ?? DateOnly.FromDateTime(DateTime.Today) : GetMondayOfWeek(startDate ?? DateOnly.FromDateTime(DateTime.Today));
            var weekNumber = ISOWeek.GetWeekOfYear(selectedStartDate.ToDateTime(new TimeOnly(12, 00)));
            
            var employees = Context.Employees.ToList();
            // var shifts = new List<Shift>(); // TODO: Get shifts from database > for day view of all employees, order by department
            var shifts = new List<Shift>
            {
                new Shift()
                {
                    Id = 1,
                    Department = Department.Vers,
                    Employee = employees[0],
                    Start = new DateTime(new DateOnly(2024, 11, 28), new TimeOnly(8, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 28), new TimeOnly(16, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                },
                new Shift()
                {
                    Id = 2,
                    Department = Department.Vakkenvullen,
                    Employee = employees[1],
                    Start = new DateTime(new DateOnly(2024, 11, 28), new TimeOnly(16, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 28), new TimeOnly(23, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                },
                new Shift()
                {
                    Id = 3,
                    Department = Department.Kassa,
                    Employee = employees[2],
                    Start = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(8, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(10, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                },
                new Shift()
                {
                    Id = 4,
                    Department = Department.Kassa,
                    Employee = employees[1],
                    Start = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(9, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(10, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                },
                new Shift()
                {
                    Id = 5,
                    Department = Department.Kassa,
                    Employee = employees[0],
                    Start = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(8, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(10, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                },
                new Shift()
                {
                    Id = 6,
                    Department = Department.Kassa,
                    Employee = null,
                    Start = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(17, 0)),
                    End = new DateTime(new DateOnly(2024, 11, 29), new TimeOnly(20, 0)),
                    IsFinal = true,
                    ShiftTakeOver = new ShiftTakeOver()
                }
            };
                
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
            return View();
        }

        public IActionResult DeleteWeek(DateOnly startDay)
        {
            return RedirectToAction("Index");
        }
    }
}
