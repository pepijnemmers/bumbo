using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ExpectationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
