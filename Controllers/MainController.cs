using System.Diagnostics;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class MainController : Controller
    {
        protected BumboDbContext _context;
        protected User? LoggedInUser;
        public MainController()
        {
            _context = new BumboDbContext();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
