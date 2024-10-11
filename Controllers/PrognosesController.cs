using Azure;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        private readonly int _pageSize = 5; //a constant for how many items per list page
        private readonly int _standardPage = 1; // a constant for the standard pagenumber
        public IActionResult Index(int? page, bool? overviewDesc)
        {
            overviewDesc = overviewDesc ?? false;
            List<WeekPrognosis> prognoses = _context.WeekPrognoses
                .OrderBy(p => p.StartDate)
                .ToList();
            List<WeekPrognosis> prognosesForPage = new List<WeekPrognosis>();

            string imageUrl = "~/img/UpArrow.png";
            if (!(bool)overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                prognoses.Reverse();
            }

            int currentPageNumber = page ?? _standardPage;
            int maxPages = (int)(Math.Ceiling((decimal)prognoses.Count / _pageSize));
            if (currentPageNumber <= 0) { currentPageNumber = _standardPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            if (prognoses.Count > 0)
            {
                for (int i = (currentPageNumber - 1) * _pageSize; i <= currentPageNumber * _pageSize && i < prognoses.Count; i++)
                {
                    prognosesForPage.Add(prognoses[i]);
                }
            }

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = _pageSize;
            ViewBag.MaxPages = maxPages;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.OverviewDesc = overviewDesc;

            return View(prognosesForPage);
        }
            public IActionResult Details(String id)
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
