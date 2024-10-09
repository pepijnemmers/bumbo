using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class DashboardController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
