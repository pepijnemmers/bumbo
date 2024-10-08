using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
