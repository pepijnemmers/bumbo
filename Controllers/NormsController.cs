using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        public IActionResult Index(int? page)
        {
            List<Norm> norms = Context.Norms.OrderBy(n => n.CreatedAt).Skip(5).ToList();
            
            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)norms.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }
            
            List<Norm> normsForPage = 
                norms
                .Skip((currentPageNumber - 1) * PageSize)
                .Take(PageSize * 5)
                .ToList();
            
            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;
            
            var viewModel = new NormsViewModel
            {
                NormsList = normsForPage,
                LatestNormsList = Context.Norms.OrderBy(n => n.CreatedAt).Take(5).ToList()
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(List<AddNormViewModel> addNormsList)
        {
            foreach (var item in addNormsList)
            {
                NotifyService.Success("test");
            }
            //if (ModelState.IsValid)
            //{
            //    using var transaction = Context.Database.BeginTransaction();

            //    try
            //    {
            //        foreach (var item in addNormsList)
            //        {
            //            var newNormEntry = new Norm
            //            {
            //                Activity = item.NormActivity,
            //                Value = item.Value,
            //                NormType = item.NormType,
            //                CreatedAt = DateTime.Now
            //            };
            //            Context.Norms.Add(newNormEntry);
            //        }
            //        Context.SaveChanges();
            //        transaction.Commit();
            //        NotifyService.Success("De normeringen zijn opgeslagen!");
            //    }
            //    catch
            //    {
            //        transaction.Rollback();
            //        NotifyService.Error("Er is iets mis gegaan");
            //    }
            //}
            return RedirectToAction("Index");
        }
    }
}
