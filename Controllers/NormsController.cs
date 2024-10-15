using BumboApp.Models;
using BumboApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Controllers
{
    public class NormsController : MainController
    {
        private static readonly int PageSize = Configuration.GetValue<int>("Pagination:DefaultPageSize");
        private static readonly int DefaultPage = Configuration.GetValue<int>("Pagination:StartPage");

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
    }
}
