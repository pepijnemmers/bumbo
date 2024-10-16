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
            if (uniqueDay.StartDate > uniqueDay.StartDate)
                return NotifyErrorAndRedirect("Het aantal verwachte klanten en verwachte coli&#39;s moet 0 of hoger zijn.", "Index");

            if (Read().Find())
                return NotifyErrorAndRedirect("Het aantal verwachte klanten en verwachte coli&#39;s moet 0 of hoger zijn.", "Index");

            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Index");

            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.Expectations.Update(expectation);
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

            return RedirectToAction("Index");
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
