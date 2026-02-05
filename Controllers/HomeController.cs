using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MermerSitesi.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using MermerSitesi.Data;
using Microsoft.EntityFrameworkCore;
using MermerSitesi.Services; // ✅ 1. EKLEME: Mail servisi için gerekli

namespace MermerSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // ✅ 2. EKLEME: Servis değişkeni

        // ✅ 3. GÜNCELLEME: Constructor'a emailService parametresi eklendi
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var allProducts = _context.Products.ToList();
            var model = new MermerSitesi.ViewModels.HomeCollectionViewModel
            {
                Travertines = allProducts.Where(p => p.Category?.ToLower() == "travertine").Take(20).ToList(),
                Marbles = allProducts.Where(p => p.Category?.ToLower() == "marble").Take(20).ToList(),
                Limestones = allProducts.Where(p => p.Category?.ToLower() == "limestone").Take(20).ToList(),
                Onyxes = allProducts.Where(p => p.Category?.ToLower() == "onyx").Take(20).ToList()
            };
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Projects()
        {
            var projeler = _context.ProjectItems.ToList();
            return View(projeler);
        }

        // --- İLETİŞİM SAYFASI (GÖRÜNTÜLEME) ---
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // ✅ 4. EKLEME: İletişim Formunu Gönderen Metot (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            try
            {
                // Mesaj içeriğini oluşturuyoruz
                string body = $"<h3>Web Sitenizden Yeni Mesaj Var!</h3>" +
                              $"<p><strong>Gönderen:</strong> {name}</p>" +
                              $"<p><strong>E-Posta:</strong> {email}</p>" +
                              $"<p><strong>Konu:</strong> {subject}</p>" +
                              $"<hr/>" +
                              $"<p><strong>Mesaj:</strong><br/>{message}</p>";

                // Maili gönder (Kime gideceğini appsettings veya buradan belirleyebilirsin)
                // Şimdilik info@rystonexport.com adresine gönderiyoruz.
                await _emailService.SendEmailAsync("info@rystonexport.com", $"Web Sitesi Mesajı: {subject}", body);

                TempData["Success"] = "Mesajınız başarıyla gönderildi! / Your message has been sent successfully!";
            }
            catch (Exception ex)
            {
                // Hata olursa loglayabiliriz veya kullanıcıya gösterebiliriz
                TempData["Error"] = "Mesaj gönderilirken bir hata oluştu. / An error occurred while sending the message.";
                // _logger.LogError(ex, "Mail gönderme hatası");
            }

            return RedirectToAction("Contact");
        }

        // --- KOLEKSİYON ---
        public IActionResult Collection(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
            ViewData["CategorySlug"] = id;

            var products = _context.Products
                                   .AsEnumerable()
                                   .Where(p => p.Category != null && p.Category.ToLower() == id.ToLower())
                                   .OrderBy(p => p.DisplayOrder)
                                   .ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            ViewData["Query"] = q;
            return View();
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture)) culture = "en-US";

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = true, Secure = true }
            );

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) return RedirectToAction("Index", "Home");
            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("privacy-policy")]
        [Route("gizlilik-politikasi")]
        public IActionResult Privacy() { return View(); }

        [Route("terms-of-use")]
        [Route("kullanim-sartlari")]
        public IActionResult Terms() { return View(); }

        [Route("sss")]
        [Route("faq")]
        public IActionResult Faq() { return View(); }

        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.ProjectItems
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }
    }
}