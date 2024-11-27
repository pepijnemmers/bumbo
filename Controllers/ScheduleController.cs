using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult DeleteWeek(DateOnly startDay)
        {
            return RedirectToAction("Index");
        }
    }
}
