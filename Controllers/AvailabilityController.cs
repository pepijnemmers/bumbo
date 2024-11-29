using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BumboApp.Controllers
{
    public class AvailabilityController : MainController
    {
        public IActionResult Index(string? id)
        {
            if (id == null)
            {
                DayOfWeek dayOfWeek = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTime.Now);
                string newId = DateTime.Now.AddDays(-(int)dayOfWeek + 1).ToString("dd-MM-yyyy");
                return RedirectToAction("Index", new { id = newId });
            }


            var dateParts = id.Split('-');
            if (dateParts.Length != 3)
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index", "Dashboard");
            }

            if (!int.TryParse(dateParts[0], out int day) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int year))
            {
                return NotifyErrorAndRedirect("Ongeldige link: de datum bevat niet numerieke waarden", "Index", "Dashboard");
            }

            DateOnly startDate;
            try
            {
                startDate = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NotifyErrorAndRedirect("Ongeldige link: de datum bestaat niet", "Index", "Dashboard");
            }

            List<Availability> availabilityList = Context.Availabilities.Where(a => a.EmployeeNumber == 1 && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList(); //Hardcoded employeeNumber

            List<SchoolSchedule> schoolScheduleList = Context.SchoolSchedules.Where(a => a.EmployeeNumber == 1 && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList(); //Hardcoded employeeNumber

            var viewModel = new AvailabilityViewModel
            {
                WeekNr = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(startDate.AddDays(3).ToDateTime(new TimeOnly(0, 0)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                StartDate = startDate,
                availabilityList = availabilityList,
                schoolScheduleList = schoolScheduleList
            };
            return View(viewModel);
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult UpdateDefault()
        {
            return View();
        }
    }
}
