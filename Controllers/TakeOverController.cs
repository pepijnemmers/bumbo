using BumboApp.Helpers;
using BumboApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            // Define start and end of the current week
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = startOfWeek.AddDays(-7);
            }
            var endOfWeek = startOfWeek.AddDays(6);

            // Fetch scheduled hours for an employee
            double GetScheduledHours(Employee employee)
            {
                if (employee == null) return 0;

                // Fetch shifts from the database
                var shifts = Context.Shifts
                    .Where(s => s.Employee.EmployeeNumber == employee.EmployeeNumber
                                && s.Start.Date >= startOfWeek
                                && s.Start.Date <= endOfWeek)
                    .ToList(); // Fetch to memory

                // Calculate total hours in memory
                return shifts
                    .Select(s => (s.End - s.Start).TotalHours)
                    .Sum();
            }

            var employeeWithShift = shiftTakeOver.Shift.Employee;
            var employeeTakingOver = shiftTakeOver.EmployeeTakingOver;

            // Calculate hours
            var hoursEmployeeWithShift = GetScheduledHours(employeeWithShift);
            var hoursEmployeeTakingOver = GetScheduledHours(employeeTakingOver);

            ViewBag.ShiftDetails = shiftTakeOver;
            ViewBag.HoursEmployeeWithShift = hoursEmployeeWithShift;
            ViewBag.ContractHoursEmployeeWithShift = employeeWithShift?.ContractHours ?? 0;
            ViewBag.HoursEmployeeTakingOver = hoursEmployeeTakingOver;
            ViewBag.ContractHoursEmployeeTakingOver = employeeTakingOver?.ContractHours ?? 0;

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
                    .ThenInclude(sto => sto.Employee)
                    .Include(sto => sto.EmployeeTakingOver)
                    .FirstOrDefault(sto => sto.ShiftId == shiftId);

                if (shiftTakeOver == null)
                {
                    return NotifyErrorAndRedirect("De overname kon niet worden gevonden", "Index", "Dashboard");
                }

                // Create notifications
                var shiftOwner = shiftTakeOver.Shift.Employee;
                var employeeTakingOver = shiftTakeOver.EmployeeTakingOver;

                shiftTakeOver.Shift.Employee = shiftTakeOver.EmployeeTakingOver;

                if (shiftOwner != null)
                {
                    var ownerNotification = new Notification
                    {
                        Employee = shiftOwner,
                        Title = "Dienst overgenomen",
                        Description = $"Je dienst op {shiftTakeOver.Shift.Start:dd-MM-yyyy} is succesvol overgenomen",
                        SentAt = DateTime.Now,
                        HasBeenRead = false,
                    };
                    Console.WriteLine("Adding notification for owner: " + ownerNotification.Description);
                    Context.Notifications.Add(ownerNotification);
                }

                if (employeeTakingOver != null)
                {
                    var takingOverNotification = new Notification
                    {
                        Employee = employeeTakingOver,
                        Title = "Dienst overgenomen",
                        Description = $"Je hebt met succes de dienst overgenomen op {shiftTakeOver.Shift.Start:dd-MM-yyyy}.",
                        SentAt = DateTime.Now,
                        HasBeenRead = false
                    };
                    Console.WriteLine("Adding notification for employee taking over: " + takingOverNotification.Description);
                    Context.Notifications.Add(takingOverNotification);
                }

                Context.ShiftTakeOvers.Remove(shiftTakeOver);

                // Save the changes to the database
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De overname is goedgekeurd", "Index", "Dashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
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
                .Include(e => e.Shifts)
                .Include(e => e.SchoolSchedules)
                .FirstOrDefault(e => e.User != null && e.User.Id == userId);
            if (employee == null) { return NotifyErrorAndRedirect("Kon gebruiker niet vinden", "Index", "Dashboard"); }

            // Retrieve the ShiftTakeOver based on the ShiftId
            var shiftTakeOver = Context.ShiftTakeOvers
                                         .Include(st => st.Shift)
                                         .FirstOrDefault(st => st.ShiftId == shiftId);
            if (shiftTakeOver == null) { return NotifyErrorAndRedirect("Kon de shift niet vinden", "Index", "Dashboard"); }
            
            // Check if the employee already has a shift at the same time
           var overlappingShiftExists = Context.Shifts
                .Include(s => s.Employee)
                .Any(s => s.Employee != null &&
                          s.Employee.EmployeeNumber == employee.EmployeeNumber &&
                          s.Start < shiftTakeOver.Shift.End &&
                          s.End > shiftTakeOver.Shift.Start);
            if (overlappingShiftExists) { return NotifyErrorAndRedirect("Je hebt al een shift op dit tijdstip", "Index", "Dashboard"); }

            int maxHours = new MaxScheduleTimeCalculationHelper(Context).GetMaxTimeCao(employee, shiftTakeOver.Shift.Department, DateOnly.FromDateTime(shiftTakeOver.Shift.Start), shiftTakeOver.Shift.Start.Hour);
            if (maxHours < (shiftTakeOver.Shift.End.Hour - shiftTakeOver.Shift.Start.Hour))
            {
                if(maxHours < 0) { maxHours = 0; }
                return NotifyErrorAndRedirect("aantal uren voldoet niet aan het maximum volgens de CAO. Het maximum aantal uren is " + maxHours, "Index", "Dashboard");
            }

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
