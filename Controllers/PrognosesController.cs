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
            int currentPageNumber = page ?? DefaultPage;
            List<WeekPrognosis> prognoses = Context.WeekPrognoses
                .OrderByDescending(p => p.StartDate)
                .ToList();

            string imageUrl = "~/img/UpArrow.png";
            if (!overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                prognoses.Reverse();
            }

            int maxPages = (int)(Math.Ceiling((decimal)prognoses.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            List<WeekPrognosis> prognosesForPage =
                prognoses
                .Skip((currentPageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
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
                return NotifyErrorAndRedirect("Ongeldige link: dd-MM-yyyy verwacht", "Index");
            }

            if (!int.TryParse(dateParts[0], out int day) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int year))
            {
                return NotifyErrorAndRedirect("Ongeldige link: de datum bevat niet numerieke waarden", "Index");
            }

            DateOnly startDate;
            try
            {
                startDate = new DateOnly(year, month, day);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NotifyErrorAndRedirect("Ongeldige link: de datum bestaat niet", "Index");
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
                Prognoses = wp?.Prognoses ?? new List<Prognosis>()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Update(List<Prognosis> prognoses)
        {
            //Validation
            if (prognoses == null || !prognoses.Any())
            {
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het bewerken van de prognose.", "Index");
            }

            using var transaction = Context.Database.BeginTransaction();
            try
            {
                foreach (Prognosis prognosis in prognoses)
                {
                    float employees = prognosis.NeededEmployees;
                    float hours = prognosis.NeededHours;
                    if (employees < 0 || hours < 0) //TODO de seeddata voldoet niet aan deze eisen (hours * 8 != employees) :( en floats zijn niet accuraat genoeg? 
                    {
                        return NotifyErrorAndRedirect("De data is ongeldig, mag niet negatief zijn", "Index");
                    }
                    if (employees * 8 != hours)
                    {
                        Console.WriteLine("" + employees + "*8 != " + hours);
                        return NotifyErrorAndRedirect("Het aantal uur moet 8 maal het aantal medewerkers zijn", "Index");
                    }
                    Prognosis existingPrognosis = Context.Prognoses
                                        .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);
                    if (existingPrognosis != null)
                    {
                        existingPrognosis.NeededHours = prognosis.NeededHours;
                        existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
                    }
                    //Context.Prognoses.Update(prognosis);
                    Context.SaveChanges();
                }

                transaction.Commit();
                NotifyService.Success("De prognose is bijgewerkt!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de prognose.");
            }
            return RedirectToAction("Index");
        }
    }
}