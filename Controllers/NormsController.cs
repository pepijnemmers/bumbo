using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        private int _amountOfNormsInSet = 5;
        public IActionResult Index(int? page)
        {
            List<Norm> norms = Context.Norms.OrderByDescending(n => n.CreatedAt).Skip(_amountOfNormsInSet).ToList();

            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)norms.Count / PageSize / _amountOfNormsInSet));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            List<Norm> normsForPage =
                norms
                .Skip((currentPageNumber - 1) * PageSize * _amountOfNormsInSet)
                .Take(PageSize * _amountOfNormsInSet)
                .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            var viewModel = new NormsViewModel
            {
                NormsList = normsForPage,
                LatestNormsList = Context.Norms.OrderByDescending(n => n.CreatedAt).Take(_amountOfNormsInSet).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(List<AddNormViewModel> addNormsList)
        {
            var latestNormsList = Context.Norms
                .OrderByDescending(n => n.CreatedAt)
                .Take(_amountOfNormsInSet)
                .ToList();

            if (latestNormsList != null) 
            { 
                bool areEqual = addNormsList.All(addNorm =>
                    latestNormsList.Any(latestNorm =>
                        latestNorm.Activity == addNorm.Activity &&
                        latestNorm.Value == addNorm.Value &&
                        latestNorm.NormType == addNorm.NormType
                    )
                );

                if (areEqual)
                {
                    return NotifyErrorAndRedirect("De ingevoerde normeringen zijn hetzelfde als de laatste normeringen.", "Index");
                }
            }
            
            using var transaction = Context.Database.BeginTransaction();

            try
            {
                var currentTime = DateTime.Now;
                foreach (var item in addNormsList)
                {
                    var newNormEntry = new Norm
                    {
                        Activity = item.Activity,
                        Value = item.Value,
                        NormType = item.NormType,
                        CreatedAt = currentTime,
                    };
                    Context.Norms.Add(newNormEntry);
                }
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect("De normeringen zijn opgeslagen.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de normeringen.", "Index");
            }
        }
    }
}
