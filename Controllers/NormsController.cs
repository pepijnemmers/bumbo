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
                NormsList = Context.Norms.OrderBy(n => n.CreatedAt).Skip(5).ToList(),
                LatestNormsList = Context.Norms.OrderBy(n => n.CreatedAt).Take(5).ToList()
            };
            return View(viewModel);

            
        }
    }
}
