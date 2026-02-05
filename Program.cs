using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using MermerSitesi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
builder.Services.AddTransient<MermerSitesi.Services.IEmailService, MermerSitesi.Services.EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// === İNGİLİZCE VARSAYILAN DİL AYARI ===
var supportedCultures = new[] { "en-US", "tr-TR", "nl-NL" };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"), // ✅ Varsayılan İngilizce
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),

    // ✅ SADECE COOKIE KONTROL ET - Tarayıcı dilini YOKSAY
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider()
    },

    // ✅ Cookie yoksa İNGİLİZCE'ye düş
    FallBackToParentCultures = false,
    FallBackToParentUICultures = false
};

app.UseRequestLocalization(localizationOptions);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();