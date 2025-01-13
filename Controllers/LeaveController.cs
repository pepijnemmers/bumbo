using BumboApp.Helpers;
using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        private Employee? _loggedInEmployee;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LeaveController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index(int? pageLeave, int? pageSickness, SortOrder? orderBy = SortOrder.Ascending, Status? selectedStatus = null)
        {
            _loggedInEmployee = GetLoggedInEmployee();

            List<LeaveRequest> leaveRequests;
            List<SickLeave> sickLeaves;
            if (LoggedInUserRole == Role.Manager)
            {
                leaveRequests = Context.LeaveRequests
                    .Where(p => (selectedStatus == null || p.Status == selectedStatus) && p.EndDate > DateTime.Now)
                    .OrderBy(p => p.StartDate)
                    .ToList();
                sickLeaves = Context.SickLeaves.OrderByDescending(p => p.Date).ToList();
            }
            else
            {
                leaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(p => selectedStatus == null || p.Status == selectedStatus)
                    .OrderBy(p => p.StartDate)
                    .ToList();
                sickLeaves = Context.SickLeaves.Where(e => e.Employee == _loggedInEmployee).OrderByDescending(p => p.Date).ToList();
            }

            if (orderBy == SortOrder.Descending)
            {
                leaveRequests.Reverse();
            }

            // pagination for leave requests
            int currentPageNumberLeave = pageLeave ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)leaveRequests.Count / PageSize));

            if (currentPageNumberLeave <= 0) { currentPageNumberLeave = DefaultPage; }
            if (currentPageNumberLeave > maxPages) { currentPageNumberLeave = maxPages; }

            ViewBag.PageNumberLeave = currentPageNumberLeave;
            ViewBag.PageSizeLeave = PageSize;
            ViewBag.MaxPagesLeave = maxPages;
            
            // pagination for sick leaves
            int currentPageNumberSickness = pageSickness ?? DefaultPage;
            int maxPagesSickness = (int)(Math.Ceiling((decimal)sickLeaves.Count / PageSize));
            
            if (currentPageNumberSickness <= 0) { currentPageNumberSickness = DefaultPage; }
            if (currentPageNumberSickness > maxPagesSickness) { currentPageNumberSickness = maxPagesSickness; }
            
            ViewBag.PageNumberSickness = currentPageNumberSickness;
            ViewBag.PageSizeSickness = PageSize;
            ViewBag.MaxPagesSickness = maxPagesSickness;

            ViewBag.OrderBy = orderBy ?? SortOrder.Ascending;

            var viewModel = new LeaveRequestViewModel
            {
                LeaveRequestsForPage = leaveRequests
                    .Skip((currentPageNumberLeave - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                SelectedStatus = selectedStatus,
                SickLeaves = sickLeaves.Skip((currentPageNumberSickness - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                LoggedInEmployee = _loggedInEmployee,
                AllEmployees = Context.Employees.Skip(1).ToList()
            };

            return View(viewModel);
        }

        public IActionResult LeaveRequest(int? id)
        {
            _loggedInEmployee = GetLoggedInEmployee();

            LeaveRequest? leaveRequest;
            int amountOfLeaveHours = 0;
            if (id == null)
            {
                leaveRequest = null;
                var leaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(s => s.Status == Status.Geaccepteerd || s.Status == Status.Aangevraagd)
                    .ToList();
                if (leaveRequests.Count != 0)
                {
                    var usedLeaveHoursThisYear = LeaveHoursCalculationHelper.CalculateLeaveHoursByYearForAllRequests(leaveRequests);
                    amountOfLeaveHours = usedLeaveHoursThisYear[DateTime.Now.Year];
                }
            }
            else
            {
                leaveRequest = Context.LeaveRequests
                          .Include(lr => lr.Employee)
                          .FirstOrDefault(lr => lr.Id == id);
            }

            var viewModel = new LeaveRequestDetailViewModel
            {
                LoggedInEmployee = _loggedInEmployee,
                LeaveRequest = leaveRequest,
                AmountOfUsedLeaveRequestHours = amountOfLeaveHours
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateLeaveRequest(LeaveRequest request)
        {
            _loggedInEmployee = GetLoggedInEmployee();
            request.Employee = _loggedInEmployee;
            var employeeLeaveRequests = Context.LeaveRequests
                    .Where(e => e.Employee == _loggedInEmployee)
                    .Where(s => s.Status == Status.Geaccepteerd || s.Status == Status.Aangevraagd)
                    .ToList();

            var amountOfLeaveHoursYear = LeaveHoursCalculationHelper.CalculateLeaveHoursByYearForAllRequests(employeeLeaveRequests);
            var amountOfLeaveHoursLeaveRequest = LeaveHoursCalculationHelper.CalculateLeaveHoursByYear(request.StartDate, request.EndDate);

            var startYearHours = amountOfLeaveHoursLeaveRequest.GetValueOrDefault(request.StartDate.Year, 0)
                     + amountOfLeaveHoursYear.GetValueOrDefault(request.StartDate.Year, 0);
            var endYearHours = amountOfLeaveHoursLeaveRequest.GetValueOrDefault(request.EndDate.Year, 0)
                               + amountOfLeaveHoursYear.GetValueOrDefault(request.EndDate.Year, 0);

            // validation 
            if (request.StartDate < DateTime.Now)
            {
                return NotifyErrorAndRedirect("Je kunt geen verlofaanvraag in het verleden doen.", "Index");
            }
            if (request.StartDate > request.EndDate || request.EndDate < request.StartDate)
            {
                return NotifyErrorAndRedirect("De einddatum moet na de startdatum zijn.", "LeaveRequest");
            }
            if (request.StartDate.Minute != 0 || request.EndDate.Minute != 0)
            {
                return NotifyErrorAndRedirect("De start- en einddatum moeten afgerond zijn op hele uren.", "LeaveRequest");
            }
            if ((request.EndDate - request.StartDate).TotalHours < 1)
            {
                return NotifyErrorAndRedirect("De minimale duur van een verlofaanvraag is 1 uur.", "LeaveRequest");
            }
            if (_loggedInEmployee == null)
            {
                return NotifyErrorAndRedirect("Er is iets misgegaan bij het ophalen van de werknemer", "Index");
            }
            if (_loggedInEmployee.LeaveHours < startYearHours || _loggedInEmployee.LeaveHours < endYearHours)
            {
                return NotifyErrorAndRedirect("Je hebt niet genoeg verlofuren om een verlofaanvraag te doen.", "Index");
            }
            if (LoggedInUserRole == Role.Manager)
            {
                return NotifyErrorAndRedirect("Je kan geen verlofaanvraag doen als manager", "Index");
            }

            bool hasOverlappingLeaveRequest = Context.LeaveRequests
                .Any(lr =>
                    lr.Employee == _loggedInEmployee &&
                    lr.StartDate <= request.EndDate &&
                    lr.EndDate >= request.StartDate);

            if (hasOverlappingLeaveRequest)
            {
                return NotifyErrorAndRedirect("Je hebt al een overlappende verlofaanvraag", "Index");
            }

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.LeaveRequests.Add(request);

                var managers = GetEmployeesInRole("Manager");
                foreach (var manager in managers)
                {
                    var notification = new Notification
                    {
                        Employee = manager,
                        Title = "Nieuwe verlofaanvraag",
                        Description = "Er is een nieuwe verlofaanvraag om te beoordelen",
                        SentAt = DateTime.Now,
                        HasBeenRead = false
                    };
                    Context.Notifications.Add(notification);
                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De verlofaanvraag is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de verlofaanvraag", "Index");
            }
        }

        public IActionResult AssessLeaveRequest(Status status, int leaveRequestId)
        {
            if (LoggedInUserRole != Role.Manager) return NotifyErrorAndRedirect("Je hebt niet de juiste rechten om deze actie te doen", "Index");
            
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                var leaveRequest = Context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .FirstOrDefault(lr => lr.Id == leaveRequestId);
                    
                var leaveRequestEmployee = Context.Employees.Find(leaveRequest?.Employee?.EmployeeNumber);
                if (leaveRequestEmployee == null || leaveRequest == null) 
                    return NotifyErrorAndRedirect("Er is iets misgegaan", "Index");
                    
                leaveRequest.Status = status;

                var notification = new Notification
                {
                    Employee = leaveRequestEmployee,
                    Title = "Nieuwe verlofaanvraag status",
                    Description = "Je verlofaanvraag is beoordeeld",
                    SentAt = DateTime.Now,
                    HasBeenRead = false
                };
                Context.Notifications.Add(notification);

                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De nieuwe verlofaanvraag status is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets misgegaan", "Index");
            }
        }

        [HttpPost]
        public IActionResult CreateSickLeave(SickLeave sickLeave)
        {
            if (sickLeave.EmployeeNumber == 0)
            {
                return NotifyErrorAndRedirect("Er was geen werknemer geselecteerd", "Index");
            }
            if (sickLeave.Date.ToDateTime(TimeOnly.MinValue) < DateTime.Now.Date)
            {
                return NotifyErrorAndRedirect("Je kunt geen ziekmelding in het verleden doen.", "Index");
            }
            if (sickLeave.Date.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date.AddDays(1))
            {
                return NotifyErrorAndRedirect("Je kunt je niet verder dan 1 dag in de toekomst ziekmelden.", "Index");
            }

            bool hasOverlappingSickLeave = Context.SickLeaves
                .Any(sl =>
                    sl.EmployeeNumber == sickLeave.EmployeeNumber &&
                    sl.Date == sickLeave.Date);

            if (hasOverlappingSickLeave)
            {
                return NotifyErrorAndRedirect("Werknemer is al ziek gemeld op deze dag", "Index");
            }

            var employee = Context.Employees.FirstOrDefault(e => e.EmployeeNumber == sickLeave.EmployeeNumber);
            if (employee == null)
            {
                return NotifyErrorAndRedirect("Er is iets misgegaan bij het ophalen van de werknemer", "Index");
            }
            sickLeave.Employee = employee;

            var employeeShifts = Context.Shifts
                                    .Where(s => s.Employee != null &&
                                                s.Employee.EmployeeNumber == sickLeave.EmployeeNumber &&
                                                s.Start.Date == sickLeave.Date.ToDateTime(TimeOnly.MinValue).Date) // checks if the shift starts on the sick leave date
                                    .ToList();

            using var transaction = Context.Database.BeginTransaction();

            try
            {
                Context.SickLeaves.Add(sickLeave);
                foreach (var shift in employeeShifts)
                {
                    var existingShiftTakeOver = Context.ShiftTakeOvers.FirstOrDefault(st => st.ShiftId == shift.Id);
                    if (existingShiftTakeOver == null)
                    {
                        var shiftTakeOver = new ShiftTakeOver
                        {
                            ShiftId = shift.Id,
                            Shift = shift,
                            Status = Status.Aangevraagd
                        };
                        shift.ShiftTakeOver = shiftTakeOver;
                        Context.ShiftTakeOvers.Add(shiftTakeOver);
                    }
                    shift.Employee = null;

                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De ziekmelding is opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de ziekmelding", "Index");
            }
        }
        
        [HttpPost]
        public IActionResult UpdateSickLeave(int employeeNumber, DateOnly date, DateOnly newDate, int newEmployee)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                
                var sickLeave = Context.SickLeaves
                    .FirstOrDefault(sl => sl.EmployeeNumber == employeeNumber && sl.Date == date);
                if (sickLeave == null) throw new Exception();

                Context.SickLeaves.Remove(sickLeave);
                
                var employee = Context.Employees.FirstOrDefault(e => e.EmployeeNumber == newEmployee);
                if (employee == null) throw new Exception();

                sickLeave = new SickLeave()
                {
                    Date = newDate,
                    Employee = employee,
                    EmployeeNumber = newEmployee
                };
                Context.SickLeaves.Add(sickLeave);
                
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De ziekmelding is bewerkt.", "Index");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het bewerken van de ziekmelding.", "Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteSickLeave(int employeeNumber, DateOnly date)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
            
                var sickLeave = Context.SickLeaves
                    .FirstOrDefault(sl => sl.EmployeeNumber == employeeNumber && sl.Date == date);
                if (sickLeave == null) throw new Exception();
                
                Context.SickLeaves.Remove(sickLeave);
                Context.SaveChanges();
                return NotifySuccessAndRedirect("De ziekmelding is verwijderd.", "Index");
            }
            catch
            {
                return NotifyErrorAndRedirect("Er is iets fout gegaan bij het verwijderen van de ziekmelding.", "Index");
            }
        }

        public Employee? GetLoggedInEmployee()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }
            var employee = Context.Employees
                .FirstOrDefault(e => e.User.Id == userId);
            return employee;
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
    }
}
