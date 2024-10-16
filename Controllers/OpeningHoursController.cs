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
            return View();
        }
    }
}
