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
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                var dbPassword = _context.Entry(user).Property("Password").CurrentValue as string;
                if (dbPassword == password)
                {
                    LoggedInUser = user;
                    return RedirectToAction("index", "Dashboard");
                }
            }
            return RedirectToAction("index", "Login");
        }

        public void Logout()
        {
            // todo: implement logout
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
