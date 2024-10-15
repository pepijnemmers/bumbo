using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        public IActionResult Index(int? page, bool overviewDesc = false)
        {
            List<WeekPrognosis> prognoses = Context.WeekPrognoses
                .OrderBy(p => p.StartDate)
                .ToList();

            List<WeekPrognosis> prognosesForPage = new List<WeekPrognosis>();
            string imageUrl = "~/img/UpArrow.png";
            if (!overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                prognoses.Reverse();
            }

            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)prognoses.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            List<WeekPrognosis> prognosesForPage = 
                prognoses
                .Skip((currentPageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = _pageSize;
            ViewBag.MaxPages = maxPages;
            
            ViewBag.ImageUrl = imageUrl;
            ViewBag.OverviewDesc = overviewDesc;

            return View(prognosesForPage);
        }
        public IActionResult Details(string id)
        {
            var dateParts = id.Split('-');
            if (dateParts.Length != 3)
            {
                return HandleInvalidInput("Ongeldige link: dd-MM-yyyy verwacht");
            }

            if (!int.TryParse(dateParts[0], out int day) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int year))
            {
                return HandleInvalidInput("Ongeldige link: de datum bevat niet numerieke waarden");
            }

            DateOnly startDate;
            try
            {
                startDate = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return HandleInvalidInput("Ongeldige link: de datum bestaat niet");
            }

            DayOfWeek dayOfWeek = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(startDate.ToDateTime(new TimeOnly(0, 0)));
            if (dayOfWeek != DayOfWeek.Monday) //Entered day is not a monday (redirect to monday of week)
            {
                string newId = startDate.AddDays(-(int)dayOfWeek + 1).ToString("dd-MM-yyyy");
                return RedirectToAction("Details", new { id = newId });
            }

            int weekNumber = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(startDate.AddDays(3).ToDateTime(new TimeOnly(0, 0)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            WeekPrognosis? wp = Context.WeekPrognoses
        .Include(wp => wp.Prognoses)
            .SingleOrDefault(wp => wp.StartDate == startDate);

            WeekPrognosisViewModel model = new WeekPrognosisViewModel
            {
                StartDate = startDate,
                WeekNr = weekNumber,
                Year = year,
                Prognoses = wp?.Prognoses
            };

            return View(model);
        }
        private IActionResult HandleInvalidInput(string errorMessage)
        {
            NotifyService.Error(errorMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(List<Prognosis> prognoses)
        {
            foreach (Prognosis prognosis in prognoses)
            {
                Prognosis existingPrognosis = Context.Prognoses
                .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);

                if (existingPrognosis != null)
                {
                    existingPrognosis.NeededHours = prognosis.NeededHours;
                    existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
                }
            }

            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
