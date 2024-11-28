using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class DashboardController : MainController
    {
        public IActionResult Index()
        {
            // Retrieve ShiftTakeOvers where the status indicates they need assessment
            var shiftTakeOvers = Context.ShiftTakeOvers
                .Include(sto => sto.Shift)           // Include related shift data
                .Where(sto => sto.Status == Status.Aangevraagd) // Filter by status
                .ToList();

            var leaveRequests = Context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == Status.Aangevraagd)
                .ToList();

            ViewBag.ShiftTakeOvers = shiftTakeOvers;
            ViewBag.LeaveRequests = leaveRequests;

            return View();
        }
    }
}
