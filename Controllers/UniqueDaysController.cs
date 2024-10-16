using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class UniqueDaysController : MainController
    {
        public IActionResult Edit(int id )
        {
            UniqueDay? uniqueDay = Context.UniqueDays.Find(id);
            return uniqueDay == null ? NotifyErrorAndRedirect("De Speciale dag die je probeert te bewerken bestaat niet.", "Index", "OpeningHours") : View(uniqueDay);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(UniqueDay uniqueDay)
        {
            // validation
            if (uniqueDay.StartDate > uniqueDay.EndDate)
                return NotifyErrorAndRedirect("EindDatum mag niet voor de StartDatum liggen.", "Index", "OpeningHours");

            if (Read().Find(u => u.StartDate == uniqueDay.StartDate && u.Name == uniqueDay.Name && uniqueDay.Id != u.Id) != null)
                return NotifyErrorAndRedirect("Deze speciale dag bestaat al.", "Index", "OpeningHours");

            if (uniqueDay.StartDate <= DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De data moet in de toekomst liggen.", "Index", "OpeningHours");

            if (!(ModelState.IsValid))
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Index", "OpeningHours");

            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                var existingDay = Context.UniqueDays.Find(uniqueDay.Id);
                if (existingDay != null)
                {
                    Context.Entry(existingDay).CurrentValues.SetValues(uniqueDay);
                }
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De Speciale dag is bijgewerkt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de Speciale dag.");
            }

            return RedirectToAction("index","OpeningHours");
        }

        [HttpGet]
        public List<UniqueDay> Read()
        {
            List<UniqueDay> uniqueDays = Context.UniqueDays
                .OrderBy(e => e.StartDate)
                .ToList();

            return uniqueDays;
        }
    }
}
