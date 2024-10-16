﻿using BumboApp.Models;
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


        [HttpPost]
        public IActionResult Update(List<Prognosis> prognoses)
        {
            if (prognoses == null || !prognoses.Any())
            {
                return HandleInvalidInput("Er is iets mis gegaan bij het bewerken van de prognose.");
            }
            //foreach (Prognosis prognosis in prognoses)
            //{


            //    if (existingPrognosis != null)
            //    {
            //        existingPrognosis.NeededHours = prognosis.NeededHours;
            //        existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
            //    }
            //}

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (Prognosis prognosis in prognoses)
                {
                    Prognosis existingPrognosis = _context.Prognoses
                        .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);
                    if (existingPrognosis == null)
                    {
                        return HandleInvalidInput("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld");
                    }
                    float employees = existingPrognosis.NeededEmployees;
                    float hours = existingPrognosis.NeededHours;
                    if (employees < 0 || hours < 0 || hours * 8 != employees) //TODO de seeddata voldoet niet aan deze eisen :( en floats zijn niet accuraat genoeg?
                    {
                        return HandleInvalidInput("De data is ongeldig");
                    }
                    _context.Prognoses.Update(existingPrognosis);
                }

                _context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De prognose is bijgewerkt!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de prognose.");
            }

            //_context.SaveChanges();
            return RedirectToAction("Index");
        }

        private IActionResult HandleInvalidInput(string errorMessage)
        {
            NotifyService.Error(errorMessage);
            return RedirectToAction("Index");
        }
    }
}
