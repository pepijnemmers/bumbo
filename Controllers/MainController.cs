using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class MainController : Controller
    {
        private BumboDbContext _context;
        public MainController()
        {
            _context = new BumboDbContext();
        }
    }
}
