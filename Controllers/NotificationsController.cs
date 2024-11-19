using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class NotificationsController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
