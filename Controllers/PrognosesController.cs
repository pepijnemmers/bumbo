using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            WeekPrognosis wp = null;
            foreach (WeekPrognosis selectedWp in _context.WeekPrognoses.Include(wp => wp.Prognoses))
            {
                if (selectedWp.Id == id) wp = selectedWp;
            }
            return View(wp);
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
