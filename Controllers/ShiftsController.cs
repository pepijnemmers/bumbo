using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ShiftsController : MainController
    {
        public IActionResult Create(DateOnly? date, TimeOnly? startTime, TimeOnly? endTime )
        {
            CheckPageAccess(Role.Manager);
            return View();
        }

        [HttpGet]
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
        
        [HttpPost]
        public IActionResult Update(int id)
        {
            // TODO update shift and redirect to schedule page to corresponding date
            return RedirectToAction("Index", "Schedule", new { startDate = "2024-12-02" });
        }
        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // TODO delete shift and redirect to schedule page to corresponding date
            return RedirectToAction("Index", "Schedule", new { startDate = "2024-12-02" });
        }
    }
}
