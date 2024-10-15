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
        public IActionResult Add(List<AddNormViewModel> nieuweNormeringen)
        {
            //using var transaction = _context.Database.BeginTransaction();

            //try
            //{
            //    foreach (var item in nieuweNormeringen)
            //    {
            //        var newNormEntry = new Norm
            //        {
            //            Activity = item.NormActivity,
            //            Value = item.Value,
            //            NormType = item.NormType,
            //            CreatedAt = DateTime.Now
            //        };
            //        _context.Norms.Add(newNormEntry);
            //    }
            //    _context.SaveChanges();
            //    transaction.Commit();
            //    NotifyService.Success("De normeringen zijn opgeslagen!");
            //}
            //catch
            //{
            //    transaction.Rollback();
            //    NotifyService.Error("Er is iets mis gegaan");
            //}

            NotifyService.Success("De normeringen zijn opgeslagen!");
            return RedirectToAction("Index");
        }      
    }
}
