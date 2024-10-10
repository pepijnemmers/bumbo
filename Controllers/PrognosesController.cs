using Azure;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        BumboDbContext DbContext = new BumboDbContext();
        public IActionResult Index(int? page)
        {
            List<WeekPrognosis> Prognoses = DbContext.WeekPrognoses.ToList();
            List<WeekPrognosis> PageList = new List<WeekPrognosis>();     

            int PageNumber = page ?? 1;
            int PageSize = 5;
            int MaxPages = (int)(Math.Ceiling((decimal)Prognoses.Count / PageSize));
            if(PageNumber <= 0) { PageNumber =1; }
            if(PageNumber > MaxPages) { PageNumber = MaxPages; }

            for(int i = (PageNumber-1)*PageSize; i <= PageNumber*PageSize && i < Prognoses.Count; i++)
            {
                PageList.Add(Prognoses[i]);
            }

            ViewBag.PageNumber = PageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = MaxPages;
            return View(PageList);
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
