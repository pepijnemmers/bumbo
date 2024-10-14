using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BumboApp.Controllers
{
    public class ExpectationsController : MainController
    {
        public IActionResult Index()
        {
            List<Expectation> expectations = Read();
            return View(expectations);
        }

        public IActionResult Edit()
        {
            return null;
        }

        [HttpGet]
        public List<Expectation> Read()
        {
            List<Expectation> allExpectations = _context.Expectations.ToList();
                
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
