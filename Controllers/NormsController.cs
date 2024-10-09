using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
