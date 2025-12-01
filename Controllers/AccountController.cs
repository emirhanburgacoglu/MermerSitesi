using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MermerSitesi.Controllers
{
    public class AccountController : Controller
    {
        // GİRİŞ SAYFASINI GÖSTER
        public IActionResult Login()
        {
            return View();
        }

        // GİRİŞ YAPMA İŞLEMİ (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Şimdilik basitçe kod içinde kontrol ediyoruz
            // İstersen burayı veritabanından kullanıcı çekecek şekilde güncelleyebiliriz
            if (username == "admin" && password == "123")
            {
                // Kimlik bilgilerini oluştur (Claims)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                // Sisteme giriş yap (Cookie oluştur)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);

                // Admin paneline yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Hatalı giriş
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // ÇIKIŞ YAPMA
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}