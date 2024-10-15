using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : MainController
    {
        private readonly int _pageSize = 5; //a constant for how many items per list page
        private readonly int _standardPage = 1;
        public IActionResult Index(int page = 1, bool overviewDesc = false,char? usePassedDates = 'n')
        {
            int maxPage;
            string imageUrl = "~/img/UpArrow.png";
            List <UniqueDay> uniqueDaysForPage = new List<UniqueDay>();

            List<UniqueDay> uniqueDays;
            if (usePassedDates == 'n')
            {
                uniqueDays = _context.UniqueDays.Where(u => u.EndDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else
            {
                uniqueDays = _context.UniqueDays.Where(u => u.EndDate < DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            uniqueDays.OrderBy(p => p.StartDate);

            if (overviewDesc)
            {
                imageUrl = "~/img/DownArrow.png";
                uniqueDays.Reverse();
            }

            maxPage = (int)Math.Ceiling((decimal)uniqueDays.Count / _pageSize);

            if (page <= 0) 
            {
                page = _standardPage;
            }
            else if (page > maxPage) 
            {
                if(maxPage > 0)
                {
                    page = maxPage;
                }
                else
                {
                    page = _standardPage;
                }
            }
            for (int i = (page - 1) * _pageSize ; i <= page * _pageSize && i < uniqueDays.Count; i++)
            {
                uniqueDaysForPage.Add(uniqueDays[i]);
            }

            ViewBag.PageNumber = page;
            ViewBag.PageSize = _pageSize;
            ViewBag.MaxPages = maxPage;
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
