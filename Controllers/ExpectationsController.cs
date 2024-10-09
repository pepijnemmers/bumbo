using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ExpectationsController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
