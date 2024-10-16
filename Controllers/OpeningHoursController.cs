using Microsoft.AspNetCore.Mvc;
using BumboApp.ViewModels;
using BumboApp.Models;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : MainController
    {
        public IActionResult Index()
        {
            var OpeningHoursViewModel = new OpeningHoursViewModel
            {
                OpeningHours = Context.OpeningHours.ToList(),
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
