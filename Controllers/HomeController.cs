using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MermerSitesi.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace MermerSitesi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Collection(string id)
{
    // Gelen kategori ismini (örn: "white", "black") View'a gönderiyoruz
    // İlerde buraya veritabanı sorgusu gelecek.
    ViewData["CategorySlug"] = id; 
    return View();
}
[HttpGet]
public IActionResult Search(string q)
{
    // Arama kutusuna yazılan metin 'q' değişkeniyle buraya gelir.
    // Şimdilik veritabanı olmadığı için sadece "Bunu aradınız: ..." diyeceğiz.
    // Backend adımında burayı gerçek ürünleri getirecek şekilde güncelleyeceğiz.
    
    ViewData["Query"] = q; // Aranan kelimeyi sayfaya taşıyoruz
    return View();
}
public IActionResult About()
{
    return View();
}

public IActionResult Blog()
{
    return View();
}

public IActionResult Contact()
{
    return View();
}
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpPost]
public IActionResult ChangeLanguage(string culture, string returnUrl)
{
    Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
    );

    return LocalRedirect(returnUrl);
}
}

