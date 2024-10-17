using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class UniqueDaysController : MainController
    {
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public List<UniqueDay> Read()
        {
            List<UniqueDay> allUniqueDays = Context.UniqueDays
                .OrderBy(u => u.StartDate)
                .ToList();

            return allUniqueDays;
        }

        [HttpPost]
        public IActionResult Create(UniqueDay uniqueDay)
        {
            // validation
            if (Read().Find(u => u.StartDate == uniqueDay.StartDate && u.Name == uniqueDay.Name) != null)
                return NotifyErrorAndRedirect("De Speciale dag die je probeert toe te voegen bestaat al.", "Index", "OpeningHours");

            if (uniqueDay.StartDate > uniqueDay.EndDate)
                return NotifyErrorAndRedirect("De startdatum moet voor of op de einddatum vallen.", "Add");

            if (uniqueDay.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De Data van de Speciale dag moet in de toekomst liggen.", "Add");

            if (uniqueDay.Factor <= 0)
                return NotifyErrorAndRedirect("de Factor moet boven 0 liggen.", "Add");

            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Add");

            // add to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.UniqueDays.Add(uniqueDay);
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De Speciale dag is toegevoegd!");
            }
            catch
            {
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het toevoegen van de Speciale dag.");
            }

            return RedirectToAction("Index","OpeningHours");
        }
    }
}

