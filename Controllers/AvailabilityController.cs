using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Claims;

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
                return RedirectToAction(nameof(Index), new { id = newId });
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

            var employee = getLoggedinEmployee();
            if (employee == null) { return View(); }
            var employeeNumber = employee.EmployeeNumber;

            List<Availability> availabilityList = Context.Availabilities.Where(a => a.EmployeeNumber == employeeNumber && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList();

            List<SchoolSchedule> schoolScheduleList = Context.SchoolSchedules.Where(a => a.EmployeeNumber == employeeNumber && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList();

            var viewModel = new AvailabilityViewModel
            {
                WeekNr = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(startDate.AddDays(3).ToDateTime(new TimeOnly(0, 0)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                StartDate = startDate,
                AvailabilityList = availabilityList,
                SchoolScheduleList = schoolScheduleList
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(string id, bool useStandard)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", nameof(Index), "Dashboard");
            }


            var employee = getLoggedinEmployee();
            if (employee == null) { return RedirectToAction(nameof(Index), new { id }); }
            var employeeNumber = employee.EmployeeNumber;

            List<Availability> availabilities = [];
            if (!useStandard)
            {
                for (int i = 0; i < 7; i++)
                {
                    var availability = new Availability
                    {
                        Date = startDate,
                        EmployeeNumber = employeeNumber,
                        StartTime = new TimeOnly(9, 0),
                        EndTime = new TimeOnly(21, 0)
                    };
                    startDate = startDate.AddDays(1);
                    availabilities.Add(availability);
                }
            }
            else
            {
                var standardAvailabilities = Context.StandardAvailabilities
                    .Where(sa => sa.EmployeeNumber == employee.EmployeeNumber);
                foreach (var standardAvailability in standardAvailabilities)
                {
                    int diffDays = ((int)standardAvailability.Day + 6) % 7;
                    var availability = new Availability
                    {
                        EmployeeNumber = employeeNumber,
                        StartTime = standardAvailability.StartTime,
                        EndTime = standardAvailability.EndTime,
                        Date = startDate.AddDays(diffDays)
                    };
                    availabilities.Add(availability);
                }
            }

            Context.Availabilities.AddRange(availabilities);
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id });
        }

        public IActionResult UpdateAvailability(string id)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", nameof(Index), "Dashboard");
            }

            var employee = getLoggedinEmployee();
            if (employee == null) { return View(); }
            var employeeNumber = employee.EmployeeNumber;

            ViewData["StartDate"] = startDate.ToString("dd-MM-yyyy");
            List<Availability> availabilityList = Context.Availabilities
                .Where(a => a.EmployeeNumber == employeeNumber && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList();

            return View(availabilityList);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvailability(List<Availability> availabilities)
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
            }
            Context.Availabilities.UpdateRange(availabilities);
            await Context.SaveChangesAsync();

            return NotifySuccessAndRedirect("Wijzigingen succesvol opgeslagen", "Index");
        }

        public async Task<IActionResult> AddSchoolSchedule(string id)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", nameof(Index), "Dashboard");
            }
            if (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                return BadRequest();
            }

            var employee = getLoggedinEmployee();
            if (employee == null) { return View(); }
            var employeeNumber = employee.EmployeeNumber;

            List<SchoolSchedule> schoolSchedules = [];
            for (int i = 0; i < 5; i++)
            {
                var ss = new SchoolSchedule
                {
                    Date = startDate,
                    EmployeeNumber = employeeNumber,
                    DurationInHours = 0,
                };
                startDate = startDate.AddDays(1);
                schoolSchedules.Add(ss);
            }

            Context.SchoolSchedules.AddRange(schoolSchedules);
            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(UpdateSchoolSchedule), new { id });
        }

        public IActionResult UpdateSchoolSchedule(string id)
        {
            DateOnly startDate;
            try
            {
                startDate = StringToDate(id);
            }
            catch
            {
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", nameof(Index), "Dashboard");
            }

            var employee = getLoggedinEmployee();
            if (employee == null) { return View(); }
            var employeeNumber = employee.EmployeeNumber;
            ViewData["EmployeeNumber"] = employeeNumber;
            ViewData["StartDate"] = startDate;

            List<SchoolSchedule> schoolSchedules = Context.SchoolSchedules
                .Where(a => a.EmployeeNumber == employeeNumber && a.Date >= startDate && a.Date < startDate.AddDays(7)).ToList();

            return View(schoolSchedules);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSchoolSchedule(List<SchoolSchedule> schedules)
        {
            if (!ModelState.IsValid)
            {
                return View(schedules);
            }

            Context.SchoolSchedules.UpdateRange(schedules);
            await Context.SaveChangesAsync();

            return NotifySuccessAndRedirect("Wijzigingen succesvol opgeslagen", "Index");
        }

        public IActionResult UpdateDefault()
        {
            return View();
        }

        //Help methods--------------------------------
        private DateOnly StringToDate(string id)
        {
            var dateParts = id.Split('-');
            if (dateParts.Length != 3)
            {
                throw new Exception();
            }

            if (!int.TryParse(dateParts[0], out int day) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int year))
            {
                throw new Exception();
            }

            DateOnly startDate;
            try
            {
                startDate = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new Exception();
            }
            return startDate;
        }

        public Employee? getLoggedinEmployee()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }
            var employee = Context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.User.Id == userId);
            return employee;
        }
    }
}
