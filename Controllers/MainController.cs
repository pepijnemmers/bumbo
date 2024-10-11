using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BumboApp.Controllers
{
    public class MainController : Controller
    {
        [FromServices] 
        protected INotyfService NotifyService { get; set; } = null!;

        private BumboDbContext _context;
        
        public MainController()
        {
            _context = new BumboDbContext();
        }
        
        /// <summary>
        /// Called before an action method is executed.
        /// Ensures that the NotifyService is set before any action method in the MainController or its derived controllers is executed.
        /// </summary>
        /// <param name="context">The context in which the action is executed.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            NotifyService = HttpContext.RequestServices.GetService<INotyfService>()!;
            base.OnActionExecuting(context);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
