using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MermerSitesi.Data;
using MermerSitesi.Models;
using System.IO;

namespace MermerSitesi.Controllers
{
    [Authorize]
    public class AdminBlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminBlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        public IActionResult Create()
        {
            return View();
        }

        // --- CREATE (RESİM YÜKLEMELİ) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleTr,ContentTr,TitleEn,ContentEn")] Blog blog, IFormFile file)
        {
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
                blog.ImageUrl = "/images/" + imageName;
            }
            else
            {
                blog.ImageUrl = "";
            }

            // Tarihi otomatik ata
            blog.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        // --- EDIT (RESİM GÜNCELLEMELİ) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleTr,ContentTr,TitleEn,ContentEn,ImageUrl,CreatedDate")] Blog blog, IFormFile? file)
        {
            if (id != blog.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
                        blog.ImageUrl = "/images/" + imageName;
                    }
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null) _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}