using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace BumboApp.Controllers
{
    public class LoginController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(User model, string password)
        {
            var user = Context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                var dbPassword = Context.Entry(user).Property("Password").CurrentValue as string;
                if (dbPassword == password)
                {
                    LoggedInUser = user;
                    return RedirectToAction("index", "Dashboard");
                }
            }
            
            NotifyService.Error("Onjuiste gebruikersnaam of wachtwoord");
            return RedirectToAction("index", "Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            LoggedInUser = null;
            NotifyService.Success("U bent uitgelogd");
            
            return RedirectToAction("index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
