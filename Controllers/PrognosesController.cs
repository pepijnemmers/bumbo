using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(string id)
        {
            var parts = id.Split('-');
            int year = int.Parse(parts[0]);
            int weekNumber = int.Parse(parts[1]);

            DateOnly mondayOfWeek = GetMondayOfWeek(year, weekNumber);

            WeekPrognosis? wp = _context.WeekPrognoses
        .Include(wp => wp.Prognoses)
            .SingleOrDefault(wp => wp.StartDate == mondayOfWeek);

            ViewData["Year"] = year;
            ViewData["WeekNr"] = weekNumber;
            

            return View(wp);
        }
        public IActionResult Update(string id)
        {
            var parts = id.Split('-');
            int year = int.Parse(parts[0]);
            int weekNumber = int.Parse(parts[1]);

            DateOnly mondayOfWeek = GetMondayOfWeek(year, weekNumber);

            WeekPrognosis? wp = _context.WeekPrognoses
        .Include(wp => wp.Prognoses)
            .SingleOrDefault(wp => wp.StartDate == mondayOfWeek);

            ViewData["Year"] = year;
            ViewData["WeekNr"] = weekNumber;

            return View(wp);
        }

        [HttpPost]
        public IActionResult Update(List<Prognosis> prognoses)
        {
            //MOdelstate?

            foreach (var prognosis in prognoses)
            {
                var existingPrognosis = _context.Prognoses
                .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);

                if (existingPrognosis != null)
                {
                    existingPrognosis.NeededHours = prognosis.NeededHours;
                    existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
                    //Console.WriteLine(existingPrognosis.NeededHours + " | " + existingPrognosis.NeededEmployees);
                }
            }

            //_context.SaveChanges();

            return RedirectToAction("Index");
        }

        public DateOnly GetMondayOfWeek(int year, int weekNumber)
        {
            DateOnly jan1 = new DateOnly(year, 1, 1);
            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            DateOnly firstMonday = jan1.AddDays(daysOffset);

            return firstMonday.AddDays((weekNumber - 1) * 7);
        }
    }
}
