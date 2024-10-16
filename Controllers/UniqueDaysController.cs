using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class UniqueDaysController : MainController
    {
        public IActionResult Edit(int id)
        {
            UniqueDay? uniqueDay = Context.UniqueDays.Find(id);
            return uniqueDay == null ? NotifyErrorAndRedirect("De Speciale dag die je probeert te bewerken bestaat niet.", "Index") : View(uniqueDay);
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
                return NotifyErrorAndRedirect("EindDatum mag niet voor de StartDatum liggen.", "Edit");

            if (Read().Find(u => u.StartDate == uniqueDay.StartDate && u.Name == uniqueDay.Name && uniqueDay.Id != u.Id) != null)
                return NotifyErrorAndRedirect("Deze speciale dag bestaat al.", "Edit");

            if (uniqueDay.StartDate < DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De data moet in de toekomst liggen.", "Edit");

            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Edit");

            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.UniqueDays.Update(uniqueDay);
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De verwachting is bijgewerkt!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de verwachting.");
            }

            return RedirectToAction("Index","OpeningHours");
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
