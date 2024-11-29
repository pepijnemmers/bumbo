using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BumboApp.Controllers
{
    public class EmployeesController : MainController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Manager);
        }
        
        public IActionResult Index(int? page)
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

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            return View(employeesForPage);
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


            ViewData["ContractHourUsed"] = "";
            ViewData["LeaveHourUsed"] = employee.leaveRequests.Where(lr => lr.Status == Status.Geaccepteerd && lr.StartDate.Year.Equals(DateTime.Now.Year));

            //var apiKey = Environment.GetEnvironmentVariable("POSTCODE_API_KEY");
            var apiKey = "669869e4-b372-430b-9b87-d41c72fcfc91";

            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    return BadRequest("API key not configured.");
            //}

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
