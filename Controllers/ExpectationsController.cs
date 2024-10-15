using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ExpectationsController : MainController
    {
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

        public IActionResult Edit(int? id, string? date)
        {
            Expectation? expectation = Context.Expectations.Find(id);
            
            if (date != null)
            {
                expectation = Context.Expectations
                    .FirstOrDefault(e => e.Date == DateOnly.FromDateTime(DateTime.Parse(date)));
            }
            
            return expectation == null ? NotifyErrorAndRedirect("De verwachting die je probeert te bewerken bestaat niet.", "Index") : View(expectation);
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
        public IActionResult Update(Expectation expectation)
        {
            // validation
            if (expectation.ExpectedCustomers < 0 || expectation.ExpectedCargo < 0)
                return NotifyErrorAndRedirect("Het aantal verwachte klanten en verwachte coli&#39;s moet 0 of hoger zijn.", "Index");
            
            if (!ModelState.IsValid)
                return NotifyErrorAndRedirect("Er is iets mis gegaan. Mogelijk zijn niet alle velden ingevuld", "Index");
            
            // update to database using transaction
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.Expectations.Update(expectation);
                Context.SaveChanges();
                transaction.Commit();
                NotifyService.Success("De verwachting is bijgewerkt!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                NotifyService.Error("Er is iets mis gegaan bij het bewerken van de verwachting.");
            }
            
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult Create(Expectation expectation)
        {
            // validation
            if (Read().Find(e => e.Date == expectation.Date) != null)
                return NotifyErrorAndRedirect("De verwachting die je probeert toe te voegen bestaat al.", "Index");

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
