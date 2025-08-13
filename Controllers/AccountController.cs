using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using EnvanterTakip.Data; // Context için
using EnvanterTakip.Models; // Personel modeli için
using Microsoft.EntityFrameworkCore;

namespace EnvanterTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly EnvanterContext _context;

        public AccountController(EnvanterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "/")
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.KullaniciAdi == username && u.Sifre == password && u.AktifMi);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.KullaniciAdi),
                    new Claim(ClaimTypes.Role, user.Rol ?? "User")
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Redirect(returnUrl);
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}