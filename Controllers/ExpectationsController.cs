using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BumboApp.Controllers
{
    public class ExpectationsController : MainController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CheckPageAccess(Role.Manager);
        }
        
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
            if (expectation.Date < DateOnly.FromDateTime(DateTime.Now))
                return NotifyErrorAndRedirect("Je kunt geen verwachtingen in het verleden bewerken.", "Index");
            
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
                return NotifySuccessAndRedirect("De verwachting is bijgewerkt.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het bewerken van de verwachting.", "Index");
            }
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
                return NotifySuccessAndRedirect("De verwachting is toegevoegd.", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de verwachting.", "Index");
            }
        }
        
        [HttpPost]
        public IActionResult BulkRead(IFormFile? csvFile, string? delimiter)
        {
            // validation
            if (csvFile == null || csvFile.Length == 0)
            {
                return NotifyErrorAndRedirect("Het bestand is leeg.", "Index");
            }
            
            if (!csvFile.FileName.EndsWith(".csv") && !csvFile.ContentType.Equals("text/csv"))
            {
                return NotifyErrorAndRedirect("Het bestand moet een <strong>.csv</strong> bestand zijn.", "Index");
            }
            
            if (delimiter != ";" && delimiter != ",")
            {
                return NotifyErrorAndRedirect("Het scheidingsteken moet een <strong>;</strong> of <strong>,</strong> zijn.", "Index");
            }
            
            List<Expectation> expectations = new List<Expectation>();
            List<Expectation> existingExpectations = Read();
            
            // read csv file
            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                bool firstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(delimiter);
                    
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    
                    // csv validation
                    if (values.Length != 3)
                    {
                        return NotifyErrorAndRedirect(
                            "Het bestand moet de volgende kolommen bevatten: <strong>Date</strong> <em>(yyyy-mm-dd)</em>, <strong>ExpectedCustomers</strong> en <strong>ExpectedCargo</strong>",
                            "Index");
                    }
                    
                    if (!DateOnly.TryParse(values[0], out var date) ||
                        !int.TryParse(values[1], out var expectedCustomers) ||
                        !int.TryParse(values[2], out var expectedCargo))
                    {
                        return NotifyErrorAndRedirect(
                            "Lege regels of ongeldige gegevens in CSV-bestand gevonden (regel: " + (expectations.Count + 1) + ")",
                            "Index");
                    }
                    
                    // value validation
                    if (existingExpectations.Find(e => e.Date == date) != null)
                    {
                        return NotifyErrorAndRedirect($"De verwachting die je probeert toe te voegen bestaat al (bij {date.ToLongDateString()})", "Index");
                    }
                    
                    if (date <= DateOnly.FromDateTime(DateTime.Now))
                    {
                        return NotifyErrorAndRedirect($"De datum van de verwachting moet in de toekomst liggen (bij {date.ToLongDateString()})", "Index");
                    }
                    
                    if (expectedCustomers < 0 || expectedCargo < 0)
                    {
                        return NotifyErrorAndRedirect($"Het aantal verwachte klanten en verwachte coli&#39;s moet 0 of hoger zijn (bij {date.ToLongDateString()}", "Index");
                    }
                    
                    expectations.Add(new Expectation
                    {
                        Date = date,
                        ExpectedCustomers = expectedCustomers,
                        ExpectedCargo = expectedCargo
                    });
                }
            }
            
            // read successful -> add to database
            return BulkCreate(expectations);
        }

        [HttpPost]
        public IActionResult BulkCreate(List<Expectation>? expectations)
        {
            if (expectations == null || expectations.Count == 0)
            {
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij de bulk invoer.", "Index");
            }
            
            using var transaction = Context.Database.BeginTransaction();
            try
            {
                Context.Expectations.AddRange(expectations);
                Context.SaveChanges();
                transaction.Commit();
                return NotifySuccessAndRedirect($"Er zijn {expectations.Count} verwachtingen toegevoegd!", "Index");
            }
            catch
            {
                transaction.Rollback();
                return NotifyErrorAndRedirect("Er is iets mis gegaan bij het toevoegen van de verwachtingen.", "Index");
            }
        }
    }
}
