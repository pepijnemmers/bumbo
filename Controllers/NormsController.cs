using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        public IActionResult Index()
        {
            var viewModel = new NormsViewModel
            {
                NormsList = _context.Norms.OrderBy(n => n.CreatedAt).Skip(5).ToList(),
                LatestNormsList = _context.Norms.OrderBy(n => n.CreatedAt).Take(5).ToList()
            };
            return View(viewModel);

            
        }
    }
}
