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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculatePrognosis(DateOnly startDate, string templateSelect) {
            //Check if an prognosis already excists for the startDate
            WeekPrognosis? existingPrognosis = Context.WeekPrognoses.FirstOrDefault(p => p.StartDate == startDate);
            if (existingPrognosis != null)
            {
                NotifyService.Warning("Er is al een prognose voor deze datum. Het is alleen mogelijk deze aan te passen.");
            }
            else {
                //Check which template is chosen
                if (templateSelect.Equals("Standaard template"))
                {
                    //Check if there excists norms and expectations
                    for (int i = 0; i < 7; i++)
                    {
                        DateOnly currentDay = startDate.AddDays(i);
                        var expectation = Context.Expectations.FirstOrDefault(e => e.Date == currentDay);
                        if (expectation == null)
                        {
                            NotifyService.Warning("Er zijn geen volledige verwachtingen voor deze week.");
                            return RedirectToAction("Index");
                        }
                    }

                    var normExists = Context.Norms.Any();
                    if (!normExists)
                    {
                        NotifyService.Warning("Er is geen normering gevonden");
                    }
                    else
                    {
                        CalculateNewPrognosis(startDate);
                    }
                }
                else
                {
                    //Check if there is a prognosis for the previous week
                    DateOnly previousWeekDate = startDate.AddDays(-7);
                    WeekPrognosis? wp = Context.WeekPrognoses.Include(wp => wp.Prognoses).SingleOrDefault(wp => wp.StartDate == previousWeekDate);
                    if (wp != null)
                    {
                        CopyPreviousPrognosis(startDate, wp);
                    }
                    else
                    {
                        NotifyService.Warning("Er bestaat geen prognose voor de voorgaande week.");
                    }
                }
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

                    int calculatedHours = (int)CalculateNeededHours(expectation, norms, (Department)j);

                    var newPrognosis = new Prognosis
                    {
                        Date = currentDate,
                        Department = (Department)j,
                        NeededHours = calculatedHours,
                        NeededEmployees = calculatedHours / 8
                    };

                    newWeekPrognosis.Prognoses.Add(newPrognosis);

                }
            }
            Context.WeekPrognoses.Add(newWeekPrognosis);
            Context.SaveChanges();
            NotifyService.Success("Er is een prognose aangemaakt op basis van het standaard template.");
        }

        private float CalculateNeededHours(Expectation expectation, List<Norm> norms, Department department)
        {
            //Vers
            if (department == Department.Vers)
            {
                var versNorm = norms.FirstOrDefault(n => n.Activity.ToString() == "Vers");
                float versValue = versNorm.Value;
                float neededHours = ((60 / versValue) * expectation.ExpectedCustomers) / 60;
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
                float neededHours = ((60 / kassaValue) * expectation.ExpectedCustomers) / 60;
                return neededHours;
            }
            return 0;
        }
    }
}