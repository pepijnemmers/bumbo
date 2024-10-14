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
                // LatestNormsList = null -> voor debugging
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add()
        {
            //var newNormEntry = new Norm
            //{
            //    Activity = norm.Key,
            //    Value = norm.Value.Value,
            //    NormType = GetNormTypeForActivity(norm.Key),
            //    CreatedAt = DateTime.Now
            //};
            //_context.Norms.Add(newNormEntry);
            // _context.SaveChanges();
            return RedirectToAction("Index");
        }      
    }
}
