using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class NormsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
