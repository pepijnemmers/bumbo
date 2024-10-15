using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class OpeningHoursController : MainController
    {
        public IActionResult Index(int? page, bool overviewDesc = false,char? usePassedDates = 'n')
        {
            int maxPage;
            string imageUrl = "~/img/UpArrow.png";
            List <UniqueDay> uniqueDaysForPage = new List<UniqueDay>();

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

            maxPage = (int)Math.Ceiling((decimal)uniqueDays.Count / PageSize);

            if (page <= 0) 
            {
                page = DefaultPage;
            }
            else if (page > maxPage) 
            {
                if(maxPage > 0)
                {
                    page = maxPage;
                }
                else
                {
                    page = DefaultPage;
                }
            }
            for (int i = ((int)page - 1) * DefaultPage; i <= page * PageSize && i < uniqueDays.Count; i++)
            {
                uniqueDaysForPage.Add(uniqueDays[i]);
            }

            ViewBag.PageNumber = page;
            ViewBag.PageSize = PageSize;
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
