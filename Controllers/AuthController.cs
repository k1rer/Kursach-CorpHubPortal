using Kursach_CorpHubPortal.Model.DTO;
using Kursach_CorpHubPortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Kursach_CorpHubPortal.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccount _accountService;

        public AuthController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _accountService.LoginAsync(model);

            if (success)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверные учетные данные");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
