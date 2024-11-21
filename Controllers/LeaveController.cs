using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class LeaveController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LeaveRequest()
        {
            return View();
        }
    }
}
