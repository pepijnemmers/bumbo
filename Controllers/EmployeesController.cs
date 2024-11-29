using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using BumboApp.ViewModels;

namespace BumboApp.Controllers
{
    public class EmployeesController : MainController
    {
        private readonly UserManager<User> _userManager;
        private HttpClient client = new HttpClient();

        public EmployeesController(UserManager<User> userManager)
        {
            _userManager = userManager;
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
            return View();
        }
        public IActionResult Update()
        {
            return View();
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
                Console.WriteLine("leave: " + leaveRequest.StartDate + " " + leaveRequest.EndDate + "||" + (leaveRequest.EndDate - leaveRequest.StartDate));
                leaveHoursUsed += (leaveRequest.EndDate - leaveRequest.StartDate).TotalHours;
            }

            var employeeRole = await GetUserRoleAsync(employee.User.Id);
            ViewData["EmployeeRole"] = employeeRole.ToFriendlyString();
            ViewData["LeaveHourUsed"] = leaveHoursUsed;

            //var apiKey = Environment.GetEnvironmentVariable("POSTCODE_API_KEY");
            var apiKey = "669869e4-b372-430b-9b87-d41c72fcfc91"; //TODO deze uit github secrets eigenlijk
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
