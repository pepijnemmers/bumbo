using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : MainController
    {
        public IActionResult Index(int? page, bool overviewDesc = false,char? usePassedDates = 'n')
        {
            int maxPages;
            int currentPageNumber = page ?? DefaultPage;
            string imageUrl = "~/img/UpArrow.png";

            List<UniqueDay> uniqueDays;
            if (usePassedDates == 'n')
            {
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else
            {
                uniqueDays = Context.UniqueDays.Where(u => u.EndDate < DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            uniqueDays.OrderBy(p => p.StartDate);

            if (overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                uniqueDays.Reverse();
            }

            maxPages = (int)Math.Ceiling((decimal)uniqueDays.Count / PageSize);
            if(maxPages <= 0) { maxPages = 1; }
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }
            List<UniqueDay> uniqueDaysForPage =
            uniqueDays
            .Skip((currentPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;
            ViewBag.ImageUrl = imageUrl;
            ViewBag.OverviewDesc = overviewDesc;
            ViewBag.UsePassedDates = usePassedDates;
            return View(uniqueDaysForPage);
        }

        public IActionResult Update()
        {
            return View();
        }
    }
}
