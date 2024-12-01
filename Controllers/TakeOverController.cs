using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class TakeOverController : MainController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Manager);
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EmployeeTakeOver(int shiftId)
        {
            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return NotifyErrorAndRedirect("Kon gebruiker niet vinden", "Index", "Dashboard"); }

            // Fetch the employee associated with the logged-in user
            var employee = Context.Employees
                .FirstOrDefault(e => e.User.Id == userId);
            if (employee == null) { return NotifyErrorAndRedirect("Kon gebruiker niet vinden", "Index", "Dashboard"); }

            // Retrieve the ShiftTakeOver based on the ShiftId
            var shiftTakeOver = Context.ShiftTakeOvers
                                         .Include(st => st.Shift)
                                         .FirstOrDefault(st => st.ShiftId == shiftId);
            if (shiftTakeOver == null) { return NotifyErrorAndRedirect("Kon de shift niet vinden", "Index", "Dashboard"); }

            shiftTakeOver.EmployeeTakingOverEmployeeNumber = employee.EmployeeNumber;
            Context.SaveChanges();

            return NotifySuccessAndRedirect("De overname is doorgegeven.", "Index", "Dashboard");
        }
    }
}
