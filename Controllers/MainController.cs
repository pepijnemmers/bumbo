using System.Diagnostics;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BumboApp.Controllers
{
    public class MainController : Controller
    {   
        [FromServices] protected INotyfService NotifyService { get; set; } = null!;
        protected readonly BumboDbContext Context;
        protected static IConfiguration Configuration = null!;
        protected static int PageSize;
        protected static int DefaultPage;

        public MainController()
        {
            Context = new BumboDbContext();
            
            // fallback values for pagination
            PageSize = 5;
            DefaultPage = 1;
        }
        
        // No Access page
        public IActionResult NoAccess()
        {
            ViewData["FromUrl"] = Request.Query["from"];
            return View();
        }
        
        // Redirects with a notification
        protected IActionResult NotifyErrorAndRedirect(string message, string redirect)
        {
            NotifyService.Error(message);
            return RedirectToAction(redirect);
        }

        protected IActionResult NotifyErrorAndRedirect(string message, string redirect, string redirectController)
        {
            NotifyService.Error(message);
            return RedirectToAction(redirect, redirectController);
        }
        
        protected IActionResult NotifySuccessAndRedirect(string message, string redirect)
        {
            NotifyService.Success(message);
            return RedirectToAction(redirect);
        }
        
        protected IActionResult NotifySuccessAndRedirect(string message, string redirect, string redirectController)
        {
            NotifyService.Success(message);
            return RedirectToAction(redirect, redirectController);
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
            
            PageSize = Configuration.GetValue<int>("Pagination:DefaultPageSize");
            DefaultPage = Configuration.GetValue<int>("Pagination:StartPage");
            
            var loggedInUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserId) && context.HttpContext.Request.Path != "/login")
            {
                context.HttpContext.Response.Redirect("/Login");
            }
            
            ViewData["User"] = LoggedInUser;
            //TODO: UNCOMMENT AND TEST -> ViewData["NumberOfNotifications"] = Context.Notification.Count(n => n.Employee.User.Id == LoggedInUser?.Id && !n.HasBeenRead) ?? 0;
        }
        
        // check if user has access to this page otherwise redirect to NoAccess page
        protected void CheckPageAccess(Role role)
        {
            if (LoggedInUser?.Role != role)
            {
                Response.Redirect("/Main/NoAccess?from=" + Request.Path);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
