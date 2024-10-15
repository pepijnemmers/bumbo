using Azure;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace BumboApp.Controllers
{
    public class PrognosesController : MainController
    {
        public IActionResult Index(int? page)
        {
            List<WeekPrognosis> tempList = new List<WeekPrognosis>();
            List<WeekPrognosis> PageList = new List<WeekPrognosis>();
            tempList.Add(new WeekPrognosis());
            tempList.Add(new WeekPrognosis());
            tempList.Add(new WeekPrognosis());
            tempList.Add(new WeekPrognosis());
            tempList.Add(new WeekPrognosis());
            tempList.Add(new WeekPrognosis());

            int PageNumber = page ?? 1;
            int PageSize = 5;
            int MaxPages = (int)(Math.Ceiling((decimal)tempList.Count / PageSize));
            if(PageNumber <= 0) { PageNumber =1; }
            if(PageNumber > MaxPages) { PageNumber = MaxPages; }

            for(int i = (PageNumber-1)*PageSize; i <= PageNumber*PageSize && i < tempList.Count; i++)
            {
                PageList.Add(tempList[i]);
            }

            ViewBag.PageNumber = PageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = MaxPages;
            return View(PageList);
        }
        private IActionResult HandleInvalidInput(string errorMessage)
        {
            NotifyService.Error(errorMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(List<Prognosis> prognoses)
        {
            foreach (Prognosis prognosis in prognoses)
            {
                Prognosis existingPrognosis = Context.Prognoses
                .Single(p => p.Department == prognosis.Department && p.Date == prognosis.Date);

                if (existingPrognosis != null)
                {
                    existingPrognosis.NeededHours = prognosis.NeededHours;
                    existingPrognosis.NeededEmployees = prognosis.NeededEmployees;
                }
            }

            Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
