using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MermerSitesi.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using MermerSitesi.Data;
using Microsoft.EntityFrameworkCore;

namespace MermerSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var allProducts = _context.Products.ToList();

            // DÜZELTME 1: Ana sayfada da harf büyüklüğüne takılmamak için ToLower() ekledik.
            // Böylece veritabanında "Travertine" de yazsa, "travertine" de yazsa bulur.
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

        public IActionResult Contact()
        {
            return View();
        }

        // --- GÜNCELLENEN VE DÜZELTİLEN KOLEKSİYON METODU ---
        public IActionResult Collection(string id)
        {
            // id boş gelirse varsayılan bir değer ata (Hata almamak için)
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            ViewData["CategorySlug"] = id;

            // DÜZELTME 2:
            // Veritabanındaki kategori ismini de, gelen id'yi de küçük harfe çevirip karşılaştırıyoruz.
            // Örn: Veritabanında "Marble" var, linkten "marble" geldi. Eşleşme sağlanır ve ürün görünür.
            var products = _context.Products
                                   .AsEnumerable() // SQLite için güvenli filtreleme
                                   .Where(p => p.Category != null && p.Category.ToLower() == id.ToLower())
                                   .OrderBy(p => p.DisplayOrder)
                                   .ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            ViewData["Query"] = q;
            // Arama sonuçlarını getirmek istersen burayı da benzer mantıkla doldurabilirsin
            return View();
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = "tr-TR";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}