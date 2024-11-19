using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class AvailabilityController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult UpdateDefault()
        {
            return View();
        }
    }
}
