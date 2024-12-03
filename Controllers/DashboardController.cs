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

            // Retrieve ShiftTakeOvers where the status indicates they need assessment and are in the future
            var shiftTakeOvers = Context.ShiftTakeOvers
                .Include(sto => sto.Shift) 
                .Where(sto => sto.Status == Status.Aangevraagd
                              && sto.EmployeeTakingOver != null
                              && sto.Shift.Start > DateTime.Now) 
                .OrderBy(sto => sto.Shift.Start) 
                .ToList();

            var shiftTakeOversEmployee = Context.ShiftTakeOvers
                .Include(sto => sto.Shift) 
                .Where(sto => sto.Status == Status.Aangevraagd
                              && sto.EmployeeTakingOver == null
                              && sto.Shift.Employee != employee
                              && sto.Shift.Start > DateTime.Now) 
                .OrderBy(sto => sto.Shift.Start) 
                .ToList();

            var leaveRequests = Context.LeaveRequests
                .Include(lr => lr.Employee) 
                .Where(lr => lr.Status == Status.Aangevraagd
                             && lr.StartDate > DateTime.Now) 
                .OrderBy(lr => lr.StartDate) 
                .ToList();

            var unreadNotifications = employee.notifications
                .Where(n => !n.HasBeenRead)
                .OrderByDescending(n => n.SentAt)
                .ToList();

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);                                                             
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = startOfWeek.AddDays(-7); 
            }
            var endOfWeek = startOfWeek.AddDays(6); 

            // Filter shifts for the current week
            var shiftsThisWeek = employee.Shifts
                .Where(s => s.Start.Date >= startOfWeek && s.Start.Date <= endOfWeek && s.IsFinal)
                .OrderBy(s => s.Start)
                .ToList();

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
