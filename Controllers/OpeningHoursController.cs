using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using BumboApp.ViewModels;
using BumboApp.Models;
using System.Collections.Generic;
using System;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : MainController
    {
        public IActionResult Index(int? page, bool overviewDesc = false, char? usePassedDates = 'n')
        {
            int currentPageNumber = page ?? DefaultPage;
            string imageUrl = "~/img/UpArrow.png";

            List<UniqueDay> uniqueDays;
            if (usePassedDates == 'n')
            {
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate >= DateOnly.FromDateTime(DateTime.Now)).OrderBy(p => p.StartDate).ToList();
            }
            else
            {
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate < DateOnly.FromDateTime(DateTime.Now)).OrderBy(p => p.StartDate).ToList();
            }

            if (overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                uniqueDays.Reverse();
            }

            int maxPages = (int)Math.Ceiling((decimal)uniqueDays.Count / PageSize);
            if (maxPages <= 0) { maxPages = 1; }
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }
            List<UniqueDay> uniqueDaysForPage =
            uniqueDays
            .Skip((currentPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.OverviewDesc = overviewDesc;
            ViewBag.UsePassedDates = usePassedDates;

            var OpeningHoursViewModel = new OpeningHoursViewModel
            {
                OpeningHours = Context.OpeningHours.ToList(),
                UniqueDays = uniqueDaysForPage
            };

            return View(OpeningHoursViewModel);
        }

        [HttpPost]
        public IActionResult Update(List<OpeningHour> openingHours)
        {
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                foreach (var openingHour in openingHours)
                {
                    if (openingHour.OpeningTime != null && openingHour.ClosingTime != null)
                    {
                        if (openingHour.OpeningTime >= openingHour.ClosingTime)
                        {
                            return NotifyErrorAndRedirect("De openingstijd kan niet na de sluitingstijd zijn", "Index");
                        }
                    }
                    else if (!(openingHour.OpeningTime == null && openingHour.ClosingTime == null))
                    {
                        return NotifyErrorAndRedirect("De openingstijd en sluitingstijd moeten ingevuld zijn", "Index");
                    }
                    Context.OpeningHours.Update(openingHour);
                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De openingstijden zijn bijgewerkt.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het bewerken van de openingstijden.", "Index");
            }
        }
    }
}
