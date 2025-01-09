using BumboApp.Models;
using System.Globalization;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        private readonly String _actionUrl = "~/Schedule?startDate="; //day will be added while making the Notification
        public IActionResult Index(string? employeeNumber, string? startDate, bool dayView = true)
        {
            // Selected employee filter or logged in employee
            Employee? selectedEmployee = null;
            if (LoggedInUserRole == Role.Employee)
            {
                selectedEmployee = Context.Employees.FirstOrDefault(employee => employee.User.Id == LoggedInUserId);
            }
            else
            {
                selectedEmployee = employeeNumber != null && int.TryParse(employeeNumber, out _) ? Context.Employees.Find(int.Parse(employeeNumber)) : null;
            }
            
            // Day or week view and selected start date
            bool isDayView = selectedEmployee == null || dayView;
            var selectedStartDate = startDate != null 
                ? (isDayView ? DateOnly.FromDateTime(DateTime.Parse(startDate)) : GetMondayOfWeek(DateOnly.FromDateTime(DateTime.Parse(startDate))))
                : (isDayView ? DateOnly.FromDateTime(DateTime.Today) : GetMondayOfWeek(DateOnly.FromDateTime(DateTime.Today)));
            int weekNumber = ISOWeek.GetWeekOfYear(selectedStartDate.ToDateTime(new TimeOnly(12, 00)));
            
            // Get all employees for filter
            var employees = Context.Employees.ToList();
            
            // Get all shifts for selected date (based on day/week view) and selected/loggedin employee
            var shifts = Context.Shifts
                .Where(shift => isDayView
                    ? DateOnly.FromDateTime(shift.Start) == selectedStartDate 
                      && (selectedEmployee == null || shift.Employee == selectedEmployee) 
                      && (LoggedInUserRole != Role.Employee || shift.IsFinal)
                    : DateOnly.FromDateTime(shift.Start) >= selectedStartDate 
                      && DateOnly.FromDateTime(shift.Start) < selectedStartDate.AddDays(7) 
                      && (selectedEmployee == null || shift.Employee == selectedEmployee) 
                      && (LoggedInUserRole != Role.Employee || shift.IsFinal))
                .OrderBy(shift => shift.Department)
                .ToList();
            
            // Check if view is concept or final
            var viewIsConcept = shifts.Any(shift => !shift.IsFinal);
            
            // Create view model and return view
            var viewModel = new ScheduleViewModel()
            {
                Role = LoggedInUserRole,
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

        public IActionResult DeleteWeek(int weekNumber, int year)
        {
            var deleteDate = new DateOnly(year, 1, 1).AddDays((weekNumber - 1) * 7);
            var shifts = Context.Shifts
                .Where(shift => DateOnly.FromDateTime(shift.Start) >= deleteDate
                                && DateOnly.FromDateTime(shift.Start) < deleteDate.AddDays(7))
                .ToList();
            
            if (shifts.Count == 0)
            {
                NotifyService.Error("Er zijn geen diensten gevonden voor de week");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }

            try
            {
                Context.ShiftTakeOvers.RemoveRange(Context.ShiftTakeOvers.Where(takeOver => shifts.Contains(takeOver.Shift)));
                Context.Shifts.RemoveRange(shifts);
                Context.SaveChanges();
                NotifyService.Success("De shifts van de week zijn verwijderd");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }
            catch
            {
                NotifyService.Error("Er is iets fout gegaan bij het verwijderen van de shifts");
                return RedirectToAction("Index", new { startDate = deleteDate.ToString("dd/MM/yyyy") });
            }
        }

        public IActionResult PublishWeek(int weekNumber, int year)
        {
            var publishDate = new DateOnly(year, 1, 1).AddDays((weekNumber - 1) * 7);
            var shifts = Context.Shifts
                .Where(shift => DateOnly.FromDateTime(shift.Start) >= publishDate
                                && DateOnly.FromDateTime(shift.Start) < publishDate.AddDays(7)
                                && !shift.IsFinal).Include(shift => shift.Employee)
                .ToList();

            if (shifts.Count == 0)
            {
                NotifyService.Error("Er zijn geen concept diensten gevonden voor de week");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }

            try
            {
                foreach (var shift in shifts)
                {
                    shift.IsFinal = true;
                    
                    // send notification to employee
                    if (shift.Employee != null)
                    {
                        Context.Notifications.Add(new Notification
                        {
                            Employee = shift.Employee,
                            Title = $"Dienst {shift.Department} toegevoegd - {shift.Start.ToString("dd-MM-yyyy")}",
                            Description = $"Er is een dienst voor jou toegevoegd op {shift.Start.ToString("dd-MM-yyyy")}",
                            SentAt = DateTime.Now,
                            HasBeenRead = false,
                            ActionUrl = _actionUrl + shift.Start.ToString("dd/MM/yyyy")
                        });
                    }
                }
                
                Context.SaveChanges();
                NotifyService.Success("De concept diensten zijn gepubliceerd");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }
            catch (Exception e)
            {
                NotifyService.Error("Er is iets fout gegaan bij het publiceren van de diensten");
                return RedirectToAction("Index", new { startDate = publishDate.ToString("dd/MM/yyyy") });
            }
            
        }
    }
}
