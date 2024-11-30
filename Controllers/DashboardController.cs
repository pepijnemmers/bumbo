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

            // Retrieve ShiftTakeOvers where the status indicates they need assessment
            var shiftTakeOvers = Context.ShiftTakeOvers
                .Include(sto => sto.Shift)           // Include related shift data
                .Where(sto => sto.Status == Status.Aangevraagd) // Filter by status
                .ToList();

            var leaveRequests = Context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == Status.Aangevraagd)
                .ToList();

            // Fetch the employee associated with the logged-in user
            var employee = Context.Employees
                .Include(e => e.notifications) // Include notifications
                .FirstOrDefault(e => e.User.Id == userId);

            var unreadNotifications = employee.notifications
                .Where(n => !n.HasBeenRead)
                .OrderByDescending(n => n.SentAt)
                .ToList();

            ViewBag.ShiftTakeOvers = shiftTakeOvers;
            ViewBag.LeaveRequests = leaveRequests;
            ViewBag.LoggedInUserRole = LoggedInUserRole;
            ViewBag.UnreadNotifications = unreadNotifications;

            return View();
        }
    }
}
