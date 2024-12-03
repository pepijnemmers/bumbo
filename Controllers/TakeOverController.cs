using BumboApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class TakeOverController : MainController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TakeOverController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Manager);
        }
        
        public IActionResult Index(int id)
        {
            // Fetch the shift and associated shift take over data
            var shiftTakeOver = Context.ShiftTakeOvers
                .Include(sto => sto.Shift)
                .ThenInclude(s => s.Employee)
                .Include(sto => sto.EmployeeTakingOver) // Include the EmployeeTakingOver
                .Where(sto => sto.ShiftId == id) // Filter by the ShiftId
                .FirstOrDefault(); // Retrieve the first match

            if (shiftTakeOver == null)
            {
                return NotifyErrorAndRedirect("Kon de shift niet vinden", "Index", "Dashboard");
            };

            ViewBag.ShiftDetails = shiftTakeOver;

            return View();
        }

        public IActionResult RejectTakeOver(int shiftId)
        {
            try
            {
                // Find the ShiftTakeOver record based on the shiftId
                var shiftTakeOver = Context.ShiftTakeOvers
                    .FirstOrDefault(sto => sto.ShiftId == shiftId);

                if (shiftTakeOver == null)
                {
                    return NotifyErrorAndRedirect("De overname kon niet worden gevonden", "Index", "Dashboard");
                }

                // Set the EmployeeTakingOverEmployeeNumber to null
                shiftTakeOver.EmployeeTakingOverEmployeeNumber = null;

                // Save the changes to the database
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De overname is afgewezen", "Index", "Dashboard");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is een fout opgetreden bij het afwijzen van de overname.", "Index", "Dashboard");
            }
        }

        public IActionResult AcceptTakeOver(int shiftId)
        {
            try
            {
                // Find the ShiftTakeOver record based on the shiftId
                var shiftTakeOver = Context.ShiftTakeOvers
                    .Include(sto => sto.Shift)
                    .Include(sto => sto.EmployeeTakingOver)
                    .FirstOrDefault(sto => sto.ShiftId == shiftId);

                if (shiftTakeOver == null)
                {
                    return NotifyErrorAndRedirect("De overname kon niet worden gevonden", "Index", "Dashboard");
                }

                shiftTakeOver.Shift.Employee = shiftTakeOver.EmployeeTakingOver;
                Context.ShiftTakeOvers.Remove(shiftTakeOver);

                // Save the changes to the database
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De overname is goedgekeurd", "Index", "Dashboard");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is een fout opgetreden bij het accepteren van de overname.", "Index", "Dashboard");
            }
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

            //TODO: Checken CAO regels.

            using var transaction = Context.Database.BeginTransaction();
            try
            {
                // Assign the employee to the shift takeover
                shiftTakeOver.EmployeeTakingOverEmployeeNumber = employee.EmployeeNumber;

                // Find all managers
                var managers = GetEmployeesInRole("Manager");

                // Create notifications for each manager
                var notifications = new List<Notification>();
                foreach (var manager in managers)
                {
                    notifications.Add(new Notification
                    {
                        Employee = manager,
                        Title = "Nieuwe shift overname",
                        Description = $"{employee.FirstName} {employee.LastName} heeft een shift overgenomen.",
                        SentAt = DateTime.Now,
                        HasBeenRead = false
                    });
                }

                // Update the ShiftTakeOver and add notifications
                Context.Notifications.AddRange(notifications);
                Context.SaveChanges();

                transaction.Commit();

                return NotifySuccessAndRedirect("De overname is doorgegeven.", "Index", "Dashboard");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is een fout opgetreden bij het verwerken van de overname.", "Index", "Dashboard");
            }
        }

        private List<Employee> GetEmployeesInRole(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName).Result;
            if (role == null) throw new Exception($"Role '{roleName}' not found.");

            var usersInRole = _userManager.GetUsersInRoleAsync(roleName).Result;

            return Context.Employees
                .Where(e => usersInRole.Select(u => u.Id).Contains(e.User.Id))
                .ToList();
        }

        public IActionResult EmployeeTakeOverRequest(int shiftId) 
        {
            var shift = Context.Shifts.FirstOrDefault(s => s.Id == shiftId);
            if (shift == null)
            {
                return NotifyErrorAndRedirect("Shift kon niet gevonden worden", "MyShifts", "Shifts");
            }

            // Check for existing takeovers for the same shift
            var existingTakeOver = Context.ShiftTakeOvers.FirstOrDefault(st => st.ShiftId == shiftId);
            if (existingTakeOver != null)
            {
                return NotifyErrorAndRedirect("De overname aanvraag bestaat al", "MyShifts", "Shifts");
            }

            var shiftTakeOver = new ShiftTakeOver
            {
                ShiftId = shiftId,
                Status = Status.Aangevraagd
            };

            try
            {
                Context.ShiftTakeOvers.Add(shiftTakeOver);
                Context.SaveChanges();
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is een fout opgetreden bij het aanmaken van de overname aanvraag", "MyShifts", "Shifts");
            }

            return RedirectToAction("MyShifts", "Shifts");
        }
    }
}
