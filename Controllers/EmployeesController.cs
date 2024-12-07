using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using BumboApp.ViewModels;
using BumboApp.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace BumboApp.Controllers
{
    public class EmployeesController : MainController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly HttpClient _client = new HttpClient();

        public EmployeesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Manager);
        }

        public async Task<Role> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return (Role)Enum.Parse(typeof(Role), roles[0]);
            }
            return Role.Unknown;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int currentPageNumber = page ?? DefaultPage;
            List<Employee> employees = Context.Employees
                .Include(e => e.User)
                .ToList();

            int maxPages = (int)(Math.Ceiling((decimal)employees.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            List<Employee> employeesForPage =
                employees
                .Skip((currentPageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            List<EmployeeIndexViewModel> viewModels = [];
            foreach (var employee in employeesForPage)
            {
                var employeeRole = await GetUserRoleAsync(employee.User.Id);
                var viewModel = new EmployeeIndexViewModel
                {
                    Id = employee.EmployeeNumber,
                    Name = employee.FirstName + " " + employee.LastName,
                    Role = employeeRole.ToFriendlyString()
                };
                viewModels.Add(viewModel);
            }

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            return View(viewModels);
        }
        public IActionResult Create()
        {
            var viewModel = new EmployeeCreateViewModel
            {
                DateOfBirth = new DateOnly(2000, 1, 1)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel.Role.Equals(Role.Unknown))
            {
                return View(viewModel);
            }

            if (viewModel.DateOfBirth <= new DateOnly(1900, 1, 1))
            {
                NotifyService.Error($"Geboortedatum moet na 1900 zijn. DEBUG: {viewModel.DateOfBirth}");
                return View(viewModel);
            }

            int minimunAge = 15; //TODO uit CAO halen (minimunleeftijd)
            if (viewModel.DateOfBirth > DateOnly.FromDateTime(DateTime.Now).AddYears(-minimunAge))
            {
                NotifyService.Error($"De minimunleeftijd bij Bumbo is {minimunAge}");
                return View(viewModel);
            }

            var roleExists = await _roleManager.RoleExistsAsync(viewModel.Role.ToString());
            if (!roleExists)
            {
                NotifyService.Error("Er is iets mis gegaan met het maken ivm de Rol");
                return View(viewModel);
            }


            var user = new User
            {
                UserName = viewModel.FirstName,
                NormalizedUserName = viewModel.FirstName.ToUpper(),
                Email = viewModel.Email,
                NormalizedEmail = viewModel.Email.ToUpper(),
                Id = Guid.NewGuid().ToString()
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, viewModel.Password);


            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, viewModel.Role.ToString());

            var employee = new Employee
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Zipcode = viewModel.Zipcode,
                HouseNumber = viewModel.HouseNumber,
                ContractHours = viewModel.ContractHours,
                LeaveHours = viewModel.LeaveHours,
                UserId = user.Id,
                StartOfEmployment = DateOnly.FromDateTime(DateTime.Now),
                StandardAvailability = new List<StandardAvailability>()
            };
            if (viewModel.Role == Role.Employee)
            {
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    employee.StandardAvailability.Add(new StandardAvailability
                    {
                        Day = day,
                        StartTime = new TimeOnly(9, 0),
                        EndTime = new TimeOnly(21, 0),
                    });
                }
            }

            Context.Employees.Add(employee);
            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var employee = Context.Employees
                .Include(e => e.User)
                .Include(e => e.LeaveRequests)
                .SingleOrDefault(e => e.EmployeeNumber == id);

            var employeeRole = await GetUserRoleAsync(employee.User.Id);

            var model = new EmployeeUpdateViewModel
            {
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                Zipcode = employee.Zipcode,
                HouseNumber = employee.HouseNumber,
                ContractHours = employee.ContractHours,
                LeaveHours = employee.LeaveHours,
                StartOfEmployment = employee.StartOfEmployment,
                Email = employee.User.Email,
                Role = employeeRole,

                IsOwner = LoggedInUserId == employee.User.Id
            };



            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, EmployeeUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = Context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.EmployeeNumber == model.EmployeeNumber);

            if (employee == null)
            {
                NotifyService.Error("Er is iets misgegaan");
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(employee.User.Id);
            if (user == null)
            {
                NotifyService.Error("Er is iets misgegaan");
                return View(model);
            }

            //Username consistent houden tussen employee en user (is nodig voor verlof en ziek)
            user.UserName = model.FirstName;
            user.NormalizedUserName = model.FirstName;
            employee.FirstName = model.FirstName;

            employee.LastName = model.LastName;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Zipcode = model.Zipcode;
            employee.HouseNumber = model.HouseNumber;
            employee.ContractHours = model.ContractHours;
            employee.LeaveHours = model.LeaveHours;
            employee.StartOfEmployment = model.StartOfEmployment;
            employee.EndOfEmployment = model.EndOfEmployment;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch
            {
                NotifyService.Error("Er is iets misgegaan bij het bewerken");
                return RedirectToAction(nameof(Details), new { id });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var employeerole = (Role)Enum.Parse(typeof(Role), roles[0]);
            if (!employeerole.Equals(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role.ToString());
                await _userManager.RemoveFromRoleAsync(user, employeerole.ToString());
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = Context.Employees
                .Include(e => e.User)
                .Include(e => e.LeaveRequests)
                .SingleOrDefault(e => e.EmployeeNumber == id);

            if (employee == null)
            {
                return NotifyErrorAndRedirect("Werknemer niet gevonden", nameof(Index));
            }

            var leaveRequests = employee.LeaveRequests
                .Where(lr => lr.Status != Status.Afgewezen).ToList();

            var usedLeaveHoursThisYear = LeaveHoursCalculationHelper.CalculateLeaveHoursByYearForAllRequests(leaveRequests);
            int amountOfLeaveHours = usedLeaveHoursThisYear[DateTime.Now.Year];

            var employeeRole = await GetUserRoleAsync(employee.User.Id);



            ViewData["IsOwner"] = LoggedInUserId == employee.User.Id;
            ViewData["EmployeeRole"] = employeeRole.ToFriendlyString();
            ViewData["LeaveHourUsed"] = amountOfLeaveHours;


            //Postcode API ----------------------------
            //var apiKey = Environment.GetEnvironmentVariable("POSTCODE_API_KEY");
            var apiKey = "";
            var apiURL = $"https://json.api-postcode.nl?postcode={employee.Zipcode}&number={employee.HouseNumber}";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", apiKey);

            try
            {
                var response = await _client.GetAsync(apiURL);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    JsonElement root = doc.RootElement;

                    ViewData["StreetName"] = root.GetProperty("street").GetString();
                    ViewData["CityName"] = root.GetProperty("city").GetString();
                }
            }
            catch (Exception e)
            {
                ViewData["StreetName"] = "-";
                ViewData["CityName"] = "-";
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Dismiss(int employeeNumber)
        {
            var employee = Context.Employees.Find(employeeNumber);
            if (employee == null)
            {
                return NotifyErrorAndRedirect("Er is iets misgegaan, werknemer niet gevonden", nameof(Index));
            }
            if (employee.EndOfEmployment != null)
            {
                NotifyService.Error("De werknemer is niet meer in dienst");
                return RedirectToAction(nameof(Details), employeeNumber);
            }

            employee.EndOfEmployment = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                NotifyService.Error("Er is iets misgegaan met het bewerken");
                return RedirectToAction(nameof(Details), employeeNumber);
            }

            var redirect = HttpContext.Request.Headers["Referer"];
            if (redirect.IsNullOrEmpty())
            {
                return RedirectToAction(nameof(Details), employeeNumber);
            }
            return Redirect(redirect);
        }
    }
}
