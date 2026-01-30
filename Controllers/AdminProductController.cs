using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MermerSitesi.Data;
using Microsoft.AspNetCore.Authorization;
using MermerSitesi.Models;
using System.IO;

namespace MermerSitesi.Controllers
{
    [Authorize]
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DÜZELTME: NameNl buraya eklendi
        public async Task<IActionResult> Create([Bind("Id,NameTr,NameEn,NameNl,Category,DisplayOrder")] Product product, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                var imageName = Guid.NewGuid() + extension;
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var location = Path.Combine(folderPath, imageName);
                using (var stream = new FileStream(location, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + imageName;
            }
            else
            {
                product.ImageUrl = "";
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: AdminProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DÜZELTME: NameNl buraya eklendi
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameTr,NameEn,NameNl,ImageUrl,Category,DisplayOrder")] Product product, IFormFile? file)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        // Yeni resim yüklendiyse işlemleri yap
                        var extension = Path.GetExtension(file.FileName);
                        var imageName = Guid.NewGuid() + extension;
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var location = Path.Combine(folderPath, imageName);
                        using (var stream = new FileStream(location, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        product.ImageUrl = "/images/" + imageName;
                    }
                    // DÜZELTME: Eğer resim yüklenmediyse, ImageUrl zaten hidden input'tan eski değeriyle geliyor olacak.

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}