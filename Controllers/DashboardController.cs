using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class DashboardController : MainController
    {
        public IActionResult Index()
        {
            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) {  return View(); }

            // Fetch the employee associated with the logged-in user
            var employee = Context.Employees
                .Include(e => e.notifications)
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.User.Id == userId);

            if (employee == null) { return View(); }

            // Retrieve ShiftTakeOvers where the status indicates they need assessment
            var shiftTakeOvers = Context.ShiftTakeOvers
                .Include(sto => sto.Shift)           // Include related shift data
                .Where(sto => sto.Status == Status.Aangevraagd && sto.EmployeeTakingOver != null) // Filter by status
                .ToList();

            var shiftTakeOversEmployee = Context.ShiftTakeOvers
                .Include(sto => sto.Shift)           // Include related shift data
                .Where(sto => sto.Status == Status.Aangevraagd && sto.EmployeeTakingOver == null && sto.Shift.Employee != employee) // Filter by status
                .ToList();

            var leaveRequests = Context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == Status.Aangevraagd)
                .ToList();

            var unreadNotifications = employee.notifications
                .Where(n => !n.HasBeenRead)
                .OrderByDescending(n => n.SentAt)
                .ToList();

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // Start of the week                                                                               // If today is Sunday, we want the week to start from the previous Monday
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = startOfWeek.AddDays(-7); // Move back to the previous Monday
            }
            var endOfWeek = startOfWeek.AddDays(6); // End of the week

            // Filter shifts for the current week
            var shiftsThisWeek = employee.Shifts
                .Where(s => s.Start.Date >= startOfWeek && s.Start.Date <= endOfWeek && s.IsFinal)
                .OrderBy(s => s.Start)
                .ToList();

            Console.WriteLine($"Start of week: {startOfWeek}");
            Console.WriteLine($"End of week: {endOfWeek}");

            foreach (var shift in employee.Shifts)
            {
                Console.WriteLine($"Shift Start: {shift.Start}");
            }

            ViewBag.ShiftTakeOvers = shiftTakeOvers;
            ViewBag.ShiftTakeOversEmployee = shiftTakeOversEmployee;
            ViewBag.LeaveRequests = leaveRequests;
            ViewBag.LoggedInUserRole = LoggedInUserRole;
            ViewBag.UnreadNotifications = unreadNotifications;
            ViewBag.ShiftsThisWeek = shiftsThisWeek;

            return View();
        }
    }
}
