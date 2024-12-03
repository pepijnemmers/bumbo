using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BumboApp.Controllers
{
    public class EmployeesController : MainController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private HttpClient client = new HttpClient();

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

            List<EmployeeIndexViewModel> viewModels = new List<EmployeeIndexViewModel>();
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
            var viewModel = new UserEmployeeViewModel
            {
                DateOfBirth = new DateOnly(2000, 1, 1)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserEmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var roleExists = await _roleManager.RoleExistsAsync(viewModel.Role.ToString());
            if (!roleExists)
            {
                NotifyService.Error("Er is iets mis gegaan met het maken ivm de Rol"); //Gebeurt als het goed is nooit
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
                UserId = user.Id
            };

            Context.Employees.Add(employee);
            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {

            var employee = Context.Employees
                .Include(e => e.User)
                .Include(e => e.leaveRequests)
                .SingleOrDefault(e => e.EmployeeNumber == id);

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
                Email = employee.User.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, EmployeeUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyService.Error("Er is iets misgegaan");
                return View(model);
            }

            var employee = Context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.EmployeeNumber == model.EmployeeNumber);

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Zipcode = model.Zipcode;
            employee.HouseNumber = model.HouseNumber;
            employee.ContractHours = model.ContractHours;
            employee.LeaveHours = model.LeaveHours;
            employee.StartOfEmployment = model.StartOfEmployment;
            employee.EndOfEmployment = model.EndOfEmployment;
            employee.User.Email = model.Email;

            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = Context.Employees
                .Include(e => e.User)
                .Include(e => e.leaveRequests)
                .SingleOrDefault(e => e.EmployeeNumber == id);

            if (employee == null)
            {
                return NotifyErrorAndRedirect("Werknemer niet gevonden", nameof(Index));
            }

            var acceptedLeaves = employee.leaveRequests
                .Where(lr => lr.Status == Status.Geaccepteerd
                && lr.StartDate.Year.Equals(DateTime.Now.Year));

            double leaveHoursUsed = 0;
            foreach (var leaveRequest in acceptedLeaves)
            {
                leaveHoursUsed += (leaveRequest.EndDate - leaveRequest.StartDate).TotalHours;
            }

            var employeeRole = await GetUserRoleAsync(employee.User.Id);
            ViewData["EmployeeRole"] = employeeRole.ToFriendlyString();
            ViewData["LeaveHourUsed"] = leaveHoursUsed;

            //var apiKey = Environment.GetEnvironmentVariable("POSTCODE_API_KEY");
            var apiKey = ""; //TODO: deze uit github secrets eigenlijk
            var apiURL = $"https://json.api-postcode.nl?postcode={employee.Zipcode}&number={employee.HouseNumber}";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("token", apiKey);

            var response = await client.GetAsync(apiURL);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;

                    ViewData["StreetName"] = root.GetProperty("street").GetString();
                    ViewData["CityName"] = root.GetProperty("city").GetString();
                }
            }

            return View(employee);
        }
    }
}
