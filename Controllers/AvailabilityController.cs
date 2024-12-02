using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace BumboApp.Controllers
{
    public class AvailabilityController : MainController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Employee);
        }

        public IActionResult Index(string? id)
        {
            if (id == null)
            {
                DayOfWeek dayOfWeek = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTime.Now);
                string newId = DateTime.Now.AddDays(-(int)dayOfWeek + 1).ToString("dd-MM-yyyy");
                return RedirectToAction("Index", new { id = newId });
            }

            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index", "Dashboard");
            }

            if (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                string newId = startDate.AddDays(-(int)startDate.DayOfWeek + 1).ToString("dd-MM-yyyy");
                return RedirectToAction("Index", new { id = newId });
            }

            List<Availability> availabilityList = Context.Availabilities.Where(a => a.EmployeeNumber == 2 && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList(); //Hardcoded employeeNumber

            List<SchoolSchedule> schoolScheduleList = Context.SchoolSchedules.Where(a => a.EmployeeNumber == 2 && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList(); //Hardcoded employeeNumber

            var viewModel = new AvailabilityViewModel
            {
                WeekNr = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(startDate.AddDays(3).ToDateTime(new TimeOnly(0, 0)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                StartDate = startDate,
                availabilityList = availabilityList,
                schoolScheduleList = schoolScheduleList
            };
            return View(viewModel);
        }

        public IActionResult Update(string id)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index", "Dashboard");
            }

            ViewData["StartDate"] = startDate.ToString("dd-MM-yyyy");
            List<Availability> availabilityList = Context.Availabilities
                .Include(a => a.Employee)
                .Where(a => a.EmployeeNumber == 2 && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList(); //Hardcoded employeeNumber

            return View(availabilityList);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string id, bool useStandard)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index", "Dashboard");
            }

            List<Availability> availabilities = [];
            if (useStandard)
            {
                for (int i = 0; i < 7; i++)
                {
                    var a = new Availability
                    {
                        Date = startDate,
                        EmployeeNumber = 2,
                        StartTime = new TimeOnly(9, 0),
                        EndTime = new TimeOnly(21, 0)
                    };
                    startDate = startDate.AddDays(1);
                    availabilities.Add(a);
                }
            }

            Context.Availabilities.AddRange(availabilities);
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Update(List<Availability> availabilities)
        {
            if (!ModelState.IsValid)
            {
                return View(availabilities);
            }

            foreach (var a in availabilities)
            {
                if (a.StartTime > a.EndTime)
                {
                    return View(availabilities);
                }
                //Console.WriteLine(a.Date + "|" + a.StartTime + " " + a.EndTime);
            }
            Context.Availabilities.UpdateRange(availabilities);
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateDefault()
        {
            return View();
        }

        private DateOnly StringToDate(string id)
        {
            var dateParts = id.Split('-');
            if (dateParts.Length != 3)
            {
                throw new Exception();
                //return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index", "Dashboard");
            }

            if (!int.TryParse(dateParts[0], out int day) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int year))
            {
                throw new Exception();
                //return NotifyErrorAndRedirect("Ongeldige link: de datum bevat niet numerieke waarden", "Index", "Dashboard");
            }

            DateOnly startDate;
            try
            {
                startDate = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new Exception();
                //return NotifyErrorAndRedirect("Ongeldige link: de datum bestaat niet", "Index", "Dashboard");
            }
            return startDate;
        }
    }
}
