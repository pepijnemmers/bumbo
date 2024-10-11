using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        public IActionResult Index(int? page, bool overviewDesc = true)
        {
            List<Norm> norms = _context.Norms.OrderByDescending(n => n.CreatedAt).ToList();
            return View(norms);
            
        }
    }
}
