﻿using BumboApp.Models;
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
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else
            {
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate < DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            uniqueDays.OrderBy(p => p.StartDate);

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
                UniqueDays = uniqueDaysForPage,
                //PageNumber = currentPageNumber,
                //PageSize = PageSize,
                //MaxPages = maxPages,
                //ImageUrl = imageUrl,
                //OverviewDesc = overviewDesc,
                //UsePassedDates = usePassedDates
            };

            return View(OpeningHoursViewModel);
        }

        [HttpPost]
        public IActionResult Update(List<OpeningHour> openingHours)
        {
            //Validation
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
            }

            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                foreach (var openingHour in openingHours)
                {
                    Context.OpeningHours.Update(openingHour);
                }
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De openingstijden zijn bijgewerkt!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de openingstijden.");
            }

            return RedirectToAction("Index");
        }
    }
}
