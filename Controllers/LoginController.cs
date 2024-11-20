using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;

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
                    return NotifySuccessAndRedirect("Je bent ingelogd", "Index", "Dashboard");
                }
            }
            
            return NotifyErrorAndRedirect("Onjuiste gebruikersnaam of wachtwoord", "Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            LoggedInUser = null;
            return NotifySuccessAndRedirect("Je bent uitgelogd", "Index", "Login");
        }
    }
}
