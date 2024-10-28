using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Data.SqlClient;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        public IActionResult Index(int? page, SortOrder? orderBy = SortOrder.Ascending)
        {
            int currentPageNumber = page ?? DefaultPage;
            List<WeekPrognosis> prognoses = Context.WeekPrognoses
                .OrderBy(p => p.StartDate)
                .ToList();

            string imageUrl = "~/img/DownArrow.png";
            if (orderBy == SortOrder.Descending)
            {
                imageUrl = "~/img/UpArrow.png";
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
            ViewBag.OrderBy = orderBy ?? SortOrder.Ascending;

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
                    if (prognosis.Date < DateOnly.FromDateTime(DateTime.Now))
                    {
                        return NotifyErrorAndRedirect("Deze prognose kan niet meer bewerkt worden", "Index");
                    }
                    if (employees < 0 || hours < 0)
                    {
                        return NotifyErrorAndRedirect("De data is ongeldig, mag niet negatief zijn", "Index");
                    }
                    if (employees * 8 != hours)
                    {
                        return NotifyErrorAndRedirect("Het aantal uur moet 8 maal het aantal medewerkers zijn", "Index");
                    }
                    Prognosis existingPrognosis = Context.Prognoses
                                        .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);
                    if (existingPrognosis != null)
                    {
                        existingPrognosis.NeededHours = prognosis.NeededHours;
                        existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
                    }
                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De prognose is bijgewerkt.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het bewerken van de prognose.", "Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculatePrognosis(DateOnly startDate, string templateSelect) {
            //Check if a prognosis already exists for the startDate
            WeekPrognosis? existingPrognosis = Context.WeekPrognoses.FirstOrDefault(p => p.StartDate == startDate);
            if (existingPrognosis != null)
            {
                return NotifyErrorAndRedirect("Er is al een prognose voor deze datum. Het is alleen mogelijk deze aan te passen.", "Index");
            }
            
            //Check which template is chosen
            if (templateSelect.Equals("Standaard template"))
            {
                //Check if there exist norms and expectations
                for (int i = 0; i < 7; i++)
                {
                    DateOnly currentDay = startDate.AddDays(i);
                    if (Context.Expectations.FirstOrDefault(e => e.Date == currentDay) == null)
                    {
                        return NotifyErrorAndRedirect("Er zijn geen (volledige) verwachtingen voor deze week.", "Index");
                    }
                }

                if (!Context.Norms.Any())
                {
                    return NotifyErrorAndRedirect("Er is geen normering gevonden.", "Index");
                }
                
                CalculateNewPrognosis(startDate);
            }
            else
            {
                //Check if there is a prognosis for the previous week
                DateOnly previousWeekDate = startDate.AddDays(-7);
                WeekPrognosis? wp = Context.WeekPrognoses.Include(wp => wp.Prognoses).SingleOrDefault(wp => wp.StartDate == previousWeekDate);
                if (wp == null)
                {
                    return NotifyErrorAndRedirect("Er bestaat geen prognose voor de voorgaande week.", "Index");
                }
                
                CopyPreviousPrognosis(startDate, wp);
            }
            
            return RedirectToAction("Index");
        }

        private void CopyPreviousPrognosis(DateOnly startDate, WeekPrognosis previousPrognosis)
        {
            var newWeekPrognosis = new WeekPrognosis
            {
                StartDate = startDate,
                Prognoses = new List<Prognosis>()
            };

            foreach (var previousPrognosisEntry in previousPrognosis.Prognoses)
            {
                var newPrognosis = new Prognosis
                {
                    Date = previousPrognosisEntry.Date.AddDays(7),
                    Department = previousPrognosisEntry.Department,
                    NeededHours = previousPrognosisEntry.NeededHours,
                    NeededEmployees = previousPrognosisEntry.NeededEmployees,
                };

                newWeekPrognosis.Prognoses.Add(newPrognosis);
            }

            Context.WeekPrognoses.Add(newWeekPrognosis);
            Context.SaveChanges();
            NotifyService.Success("Er is een prognose aangemaakt op basis van de voorgaande week.");
        }

        private void CalculateNewPrognosis(DateOnly startDate)
        {
            //Finding the latest norms
            var latestNormDate = Context.Norms.Max(n => n.CreatedAt);
            var norms = Context.Norms.Where(n => n.CreatedAt == latestNormDate).ToList();

            var newWeekPrognosis = new WeekPrognosis
            {
                StartDate = startDate,
                Prognoses = new List<Prognosis>()
            };

            //loop through the days
            for (int i = 0; i < 7; i++)
            {
                //loop through the departments
                for (int j = 0; j < 3; j++)
                {
                    DateOnly currentDate = startDate.AddDays(i);

                    var expectation = Context.Expectations.FirstOrDefault(e => e.Date == currentDate);

                    int calculatedHours = (int)CalculateNeededHours(expectation, norms, (Department)j, currentDate);

                    var newPrognosis = new Prognosis
                    {
                        Date = currentDate,
                        Department = (Department)j,
                        NeededHours = calculatedHours,
                        NeededEmployees = (float)(calculatedHours / 8.0)
                    };

                    newWeekPrognosis.Prognoses.Add(newPrognosis);

                }
            }
            Context.WeekPrognoses.Add(newWeekPrognosis);
            Context.SaveChanges();
            NotifyService.Success("Er is een prognose aangemaakt op basis van het standaard template.");
        }

        private float CalculateNeededHours(Expectation expectation, List<Norm> norms, Department department, DateOnly currentDate)
        {
            float factor = 1;
            var uniqueDay = Context.UniqueDays.FirstOrDefault(ud => currentDate >= ud.StartDate && currentDate <= ud.EndDate);
            if (uniqueDay != null) { 
                factor = uniqueDay.Factor;
            }

            //Vers
            if (department == Department.Vers)
            {
                var versNorm = norms.FirstOrDefault(n => n.Activity.ToString() == "Vers");
                float versValue = versNorm.Value;
                float neededHours = ((60 / versValue) * (expectation.ExpectedCustomers * factor)) / 60;
                return neededHours;
            }
            //Vakenvullen
            else if (department == Department.Vakkenvullen)
            {
                int shelfMeters = 100;
                //var coliUitladenNorm = norms.FirstOrDefault(n => n.Activity.ToString() == "Coli uitladen");
                var coliUitladenNorm = norms.FirstOrDefault(n => n.Activity == NormActivity.ColiUitladen);
                float coliUitladenValue = coliUitladenNorm.Value;

                var vakkenVullenNorm = norms.FirstOrDefault(n => n.Activity == NormActivity.VakkenVullen);
                float vakkenVullenValue = vakkenVullenNorm.Value;

                var spiegelenNorm = norms.FirstOrDefault(n => n.Activity.ToString() == "Spiegelen");
                float spiegelenValue = spiegelenNorm.Value;

                float needeHours = ((vakkenVullenValue * expectation.ExpectedCargo) + coliUitladenValue + ((spiegelenValue * shelfMeters) / 60))/60;
                return needeHours;
            }
            //Kassa
            else if (department == Department.Kassa)
            {
                var kassaNorm = norms.FirstOrDefault(n => n.Activity.ToString() == "Kassa");
                float kassaValue = kassaNorm.Value;
                float neededHours = ((60 / kassaValue) * (expectation.ExpectedCustomers * factor)) / 60;
                return neededHours;
            }
            return 0;
        }
    }
}