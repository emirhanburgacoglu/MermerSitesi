using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using MermerSitesi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Servisi (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC ve Dil Servisleri
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// KİMLİK DOĞRULAMA SERVİSİ
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
} // <--- DİKKAT: if bloğu burada kapanmalı!

app.UseHttpsRedirection();
app.UseStaticFiles();

// --- DİL AYARLARI ---
// Bu değişkeni if bloğunun DIŞINA aldık, artık herkes görebilir.
// Not: Resource dosyaların (SharedResource.nl.resx) ile buradaki kodlar (nl veya nl-NL) uyumlu olmalı.
// Genelde sadece "tr", "en", "nl" kullanmak daha garantidir ama dosya adın en-US ise böyle kalsın.
var supportedCultures = new[] { "tr-TR", "en-US", "nl-NL" }; 

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("tr-TR")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
// --------------------

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// .NET sürümüne göre burası değişebilir ama UseStaticFiles varken MapStaticAssets opsiyoneldir.
// Hata verirse bu satırı silebilirsin: app.MapStaticAssets(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();