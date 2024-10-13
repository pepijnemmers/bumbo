using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        private readonly int _pageSize = 5; //a constant for how many items per list page
        private readonly int _standardPage = 1; // a constant for the standard pagenumber
        public IActionResult Index(int? page, bool overviewDesc = false)
        {
            List<WeekPrognosis> prognoses = _context.WeekPrognoses
                .OrderBy(p => p.StartDate)
                .ToList();
            List<WeekPrognosis> prognosesForPage = new List<WeekPrognosis>();

            string imageUrl = "~/img/UpArrow.png";
            if (!overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                prognoses.Reverse();
            }

            int currentPageNumber = page ?? _standardPage;
            int maxPages = (int)(Math.Ceiling((decimal)prognoses.Count / _pageSize));
            if (currentPageNumber <= 0) { currentPageNumber = _standardPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            if (prognoses.Count > 0)
            {
                for (int i = (currentPageNumber - 1) * _pageSize; i <= currentPageNumber * _pageSize && i < prognoses.Count; i++)
                {
                    prognosesForPage.Add(prognoses[i]);
                }
            }

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = _pageSize;
            ViewBag.MaxPages = maxPages;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.OverviewDesc = overviewDesc;

            return View(prognosesForPage);
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
