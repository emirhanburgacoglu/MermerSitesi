using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography; // Şifreleme için gerekli
using System.Text;

namespace MermerSitesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        // Ayar dosyasını (appsettings.json) okumak için bunu ekliyoruz
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GİRİŞ SAYFASI
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa panele yönlendir
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AdminProject");
            }
            return View();
        }

        // GİRİŞ İŞLEMİ (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Ayar dosyasından doğru bilgileri al
            var correctEmail = _configuration["AdminSettings:Email"];
            var correctHash = _configuration["AdminSettings:PasswordHash"];

            // 2. Girilen şifreyi Hash'e çevir
            var inputHash = ComputeSha256Hash(password);

            // 3. Kontrol Et (E-posta ve Şifre Hash'i tutuyor mu?)
            if (email == correctEmail && inputHash == correctHash)
            {
                // Kimlik bilgilerini oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Beni hatırla
                    ExpiresUtc = DateTime.UtcNow.AddHours(2) // 2 saat sonra oturum düşsün
                };

                // Çerezi (Cookie) oluştur
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "AdminProject");
            }

            // Hatalı giriş
            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        // ÇIKIŞ YAPMA
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // --- YARDIMCI METOD: Şifreyi Şifreleyen Fonksiyon ---
        private string ComputeSha256Hash(string rawData)
        {
            if (string.IsNullOrEmpty(rawData)) return "";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}