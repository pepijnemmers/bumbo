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
        public IActionResult Create(Expectation expectation)
        {
            // validation
            if (false)
            {
                // TODO validate if there is already an expectation for this date
            }

            if (expectation.Date <= DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("De datum van de verwachting moet in de toekomst liggen.", "Index");

            if (expectation.ExpectedCustomers < 0 || expectation.ExpectedCargo < 0)
                return NotifyErrorAndRedirect("Het aantal verwachte klanten en verwachte coli&#39;s moet 0 of hoger zijn.", "Index");
            
            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Index");
            
            // add to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.Expectations.Add(expectation);
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De verwachting is toegevoegd!");
            }
            catch
            {
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het toevoegen van de verwachting.");
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void BulkCreate(List<Expectation> expectations)
        {

        }
    }
}
