using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
