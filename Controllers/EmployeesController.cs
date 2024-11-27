using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BumboApp.Controllers
{
    public class EmployeesController : MainController
    {
        static readonly HttpClient client = new HttpClient(); //TODO DIT MOET HIER WEG TEST


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
                .SingleOrDefault(e => e.EmployeeNumber == id);

            if (employee == null)
            {
                return NotifyErrorAndRedirect("Werknemer niet gevonden", nameof(Index));
            }

            var apiURL = $"https://json.api-postcode.nl?postcode={employee.Zipcode}&number={employee.HouseNumber}";

            var allEnvVars = Environment.GetEnvironmentVariables();
            foreach (var key in allEnvVars.Keys)
            {
                Console.WriteLine($"{key}: {allEnvVars[key]}");
            }

            //var apiKey = Environment.GetEnvironmentVariable("POSTCODE_API_KEY");

            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    return BadRequest("API key not configured.");
            //}

            client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");


            return View(employee);
        }
    }
}
