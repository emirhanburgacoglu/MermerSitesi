using Microsoft.EntityFrameworkCore;
using MermerSitesi.Models;

namespace MermerSitesi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanı Tablolarımız
        public DbSet<Product> Products { get; set; }
        public DbSet<ProjectItem> ProjectItems { get; set; }
    }
}