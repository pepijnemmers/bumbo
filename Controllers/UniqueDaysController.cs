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
    }
}
