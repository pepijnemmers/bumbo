using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ShiftsController : MainController
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult MyShifts()
        {
            return View();
        }
    }
}
