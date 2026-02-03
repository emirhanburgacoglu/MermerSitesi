using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MermerSitesi.Data;
using MermerSitesi.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MermerSitesi.Controllers
{
    [Authorize]
    public class AdminProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminProject
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectItems.ToListAsync());
        }

        // GET: AdminProject/Details/5
        // GET: AdminProject/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Projeyi ve Galeri Resimlerini veritabanından çekiyoruz
            var projectItem = await _context.ProjectItems
                .Include(p => p.Images) // <--- GALERİ RESİMLERİNİ DAHİL ET
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectItem == null) return NotFound();

            return View(projectItem);
        }

        // GET: AdminProject/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminProject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DİKKAT: "file" yerine "files" (liste) alıyoruz.
        public async Task<IActionResult> Create([Bind("Id,TitleTr,TitleEn,TitleNl,Country")] ProjectItem projectItem, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // 1. Önce Projeyi Kaydet (ID oluşsun diye)
                // Eğer ilk resim varsa onu kapak resmi yapalım
                if (files != null && files.Count > 0)
                {
                    // İlk resmi kapak yapma mantığı aşağıda işlenecek
                }

                _context.Add(projectItem);
                await _context.SaveChangesAsync(); // Burada projenin ID'si oluştu

                // 2. Resimleri Yükle ve Galeriye Ekle
                if (files != null && files.Count > 0)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/projects");
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    // İlk dosya kapak resmi olsun (ImageUrl)
                    bool isFirst = true;

                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var imageName = Guid.NewGuid() + extension;
                        var fullPath = Path.Combine(folderPath, imageName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        if (isFirst)
                        {
                            // Kapak resmi
                            projectItem.ImageUrl = "/images/projects/" + imageName;
                            _context.Update(projectItem);
                            isFirst = false;
                        }
                        else
                        {
                            // Diğer resimler galeriye
                            var galleryImage = new ProjectImage
                            {
                                ImageUrl = "/images/projects/" + imageName,
                                ProjectItemId = projectItem.Id
                            };
                            _context.ProjectImages.Add(galleryImage);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(projectItem);
        }
        // GET: AdminProject/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var projectItem = await _context.ProjectItems.FindAsync(id);
            if (projectItem == null) return NotFound();
            return View(projectItem);
        }

        // POST: AdminProject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DÜZELTME 2: Edit metoduna da "TitleNl" ve "Country" EKLENDİ.
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleTr,TitleEn,TitleNl,Country,ImageUrl,CreatedDate")] ProjectItem projectItem, IFormFile? file)
        {
            if (id != projectItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Eski resmi korumak için veritabanından mevcut hali çekilebilir ama 
                    // burada hidden input (ImageUrl) ile geldiği varsayılıyor.

                    if (file != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var imageName = Guid.NewGuid() + extension;
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        using (var stream = new FileStream(Path.Combine(folderPath, imageName), FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        projectItem.ImageUrl = "/images/" + imageName;
                    }

                    _context.Update(projectItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectItemExists(projectItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectItem);
        }

        // GET: AdminProject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var projectItem = await _context.ProjectItems.FirstOrDefaultAsync(m => m.Id == id);
            if (projectItem == null) return NotFound();
            return View(projectItem);
        }

        // POST: AdminProject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectItem = await _context.ProjectItems.FindAsync(id);
            if (projectItem != null) _context.ProjectItems.Remove(projectItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectItemExists(int id)
        {
            return _context.ProjectItems.Any(e => e.Id == id);
        }
    }
}