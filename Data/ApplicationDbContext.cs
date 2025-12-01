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
        public DbSet<Blog> Blogs { get; set; }
    }
}