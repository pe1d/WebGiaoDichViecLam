using Microsoft.EntityFrameworkCore;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ProductModel>()
            //     .HasKey(p => p.ProductId);
            // base.OnModelCreating(modelBuilder);
        }

        // public DbSet<ProductModel> tblProducts { get; set; }
    }
}
