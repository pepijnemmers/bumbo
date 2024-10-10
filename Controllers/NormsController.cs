using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        public IActionResult Index()
        {
            var norms = _context.Norms.ToList();
            return View(norms);
        }
    }
}
