using Azure;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        BumboDbContext DbContext = new BumboDbContext();
        public IActionResult Index(int? page, bool? overviewDesc)
        {
            overviewDesc = overviewDesc ?? false;
            List<WeekPrognosis> prognoses = DbContext.WeekPrognoses
                .OrderBy(p => p.StartDate)
                .ToList();
            List<WeekPrognosis> prognosesForPage = new List<WeekPrognosis>();

            string ImageUrl = "~/img/DownArrow.png";
            if ((bool)overviewDesc)
            {
                ImageUrl = "~/img/UpArrow.png";
                prognoses.Reverse();
            }

            int currentPageNumber = page ?? 1;
            int pageListSized = 5;
            int maxPages = (int)(Math.Ceiling((decimal)prognoses.Count / pageListSized));
            if(currentPageNumber <= 0) { currentPageNumber = 1; }
            if(currentPageNumber > maxPages) { currentPageNumber = maxPages; }

            for(int i = (currentPageNumber - 1)* pageListSized; i <= currentPageNumber * pageListSized && i < prognoses.Count; i++)
            {
                prognosesForPage.Add(prognoses[i]);
            }

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = pageListSized;
            ViewBag.MaxPages = maxPages;
            ViewBag.ImageUrl = ImageUrl;
            ViewBag.OverviewDesc = overviewDesc;
            return View(prognosesForPage);
        }

        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
