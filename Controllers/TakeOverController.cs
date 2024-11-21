using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class TakeOverController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
