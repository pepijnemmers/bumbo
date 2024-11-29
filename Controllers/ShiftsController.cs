using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ShiftsController : MainController
    {
        public IActionResult Create()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult Update()
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        public IActionResult MyShifts()
        {
            CheckPageAccess(Role.Employee);
            return View();
        }
    }
}
