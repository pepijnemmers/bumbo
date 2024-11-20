using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ProfileController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
