using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ExpectationsController : MainController
    {
        private static readonly int PageSize = Configuration.GetValue<int>("Pagination:DefaultPageSize");
        private static readonly int DefaultPage = Configuration.GetValue<int>("Pagination:StartPage");
        
        public IActionResult Index(int? page)
        {
            List<Expectation> expectations = Read();
            
            int currentPageNumber = page ?? DefaultPage;
            int maxPages = (int)(Math.Ceiling((decimal)expectations.Count / PageSize));
            if (currentPageNumber <= 0) { currentPageNumber = DefaultPage; }
            if (currentPageNumber > maxPages) { currentPageNumber = maxPages; }
            
            List<Expectation> expectationsForPage = 
                expectations
                .Skip((currentPageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.PageNumber = currentPageNumber;
            ViewBag.PageSize = PageSize;
            ViewBag.MaxPages = maxPages;

            return View(expectationsForPage);
        }

        public IActionResult Edit()
        {
            return null;
        }

        [HttpGet]
        public List<Expectation> Read()
        {
            List<Expectation> allExpectations = Context.Expectations
                .OrderBy(e => e.Date)
                .ToList();
                
            return allExpectations;
        }

        [HttpPost]
        public void Update(Expectation expectation)
        {
            
        }
        
        [HttpPost]
        public void Create(Expectation expectation)
        {
            
        }
        
        [HttpPost]
        public void Create(List<Expectation> expectations)
        {
            
        }
    }
}
