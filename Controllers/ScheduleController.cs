using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ScheduleController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }
    }
}
