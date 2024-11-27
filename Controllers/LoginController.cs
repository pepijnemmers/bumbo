using BumboApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace BumboApp.Controllers
{
    public class LoginController : MainController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    LoggedInUser = user;
                    return NotifySuccessAndRedirect("Je bent ingelogd", "Index", "Dashboard");
                }
            }
            
            return NotifyErrorAndRedirect("Onjuiste gebruikersnaam of wachtwoord", "Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NotifySuccessAndRedirect("Je bent uitgelogd", "Index", "Login");
        }
    }
}
