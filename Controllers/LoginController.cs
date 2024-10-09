using BumboApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BumboApp.Controllers
{
    public class LoginController : MainController
    {
        public IActionResult Index()
        {
            return View();
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
