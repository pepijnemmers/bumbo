using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BumboApp.Controllers
{
    public class MainController : Controller
    {
        private static User? _loggedInUser;
        
        [FromServices] protected INotyfService NotifyService { get; set; } = null!;
        protected readonly BumboDbContext Context;
        protected static IConfiguration Configuration = null!;
        protected static User? LoggedInUser
        {
            get => _loggedInUser;
            set => _loggedInUser = value;
        }

        public MainController()
        {
            Context = new BumboDbContext();
        }
        
        /// <summary>
        /// Called before an action method is executed.
        /// Ensures that the NotifyService is set before any action method in the MainController or its derived controllers is executed.
        /// </summary>
        /// <param name="context">The context in which the action is executed.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {    
            base.OnActionExecuting(context);
            NotifyService = HttpContext.RequestServices.GetService<INotyfService>()!;
            Configuration = HttpContext.RequestServices.GetService<IConfiguration>()!;
            
            if (LoggedInUser == null && context.HttpContext.Request.Path != "/login")
            {
                context.HttpContext.Response.Redirect("/Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
