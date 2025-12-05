using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MermerSitesi.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using MermerSitesi.Data; // Veritabanı için gerekli
using Microsoft.EntityFrameworkCore; // Veritabanı sorguları için gerekli

namespace MermerSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Veritabanı Köprüsü

        // Constructor'da veritabanını içeri alıyoruz (Dependency Injection)
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Projects()
        {
            var projeler = _context.ProjectItems.ToList();
            return View(projeler); // Listeyi View'ın içine koymalısın!
        }
      

        public IActionResult Contact()
        {
            return View();
        }

        // --- GÜNCELLENEN KOLEKSİYON METODU ---
        public IActionResult Collection(string id)
        {
            // 1. Kategori başlığı için slug'ı View'a gönder
            ViewData["CategorySlug"] = id;

            // 2. Veritabanından o kategoriye ait ürünleri çek
            // Örn: Kategori "white" ise sadece beyazları getir ve sıraya diz.
            var products = _context.Products
                                   .Where(p => p.Category == id)
                                   .OrderBy(p => p.DisplayOrder)
                                   .ToList();

            // 3. Ürün listesini sayfaya gönder
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
    // Gelen dil kodu (culture) boşsa veya null ise varsayılan olarak tr-TR yap
    if (string.IsNullOrEmpty(culture))
    {
        culture = "tr-TR";
    }

    // Dil tercihini çerezlere (Cookie) kaydet
    Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
    );

    // Kullanıcıyı geldiği sayfaya geri gönder
    return LocalRedirect(returnUrl);
}
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}