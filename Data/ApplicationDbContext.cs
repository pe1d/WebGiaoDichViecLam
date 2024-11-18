using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebGiaoDichViecLam.Models;

namespace WebGiaoDichViecLam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<tblAccount> tblAccount { get; set; }
        public DbSet<tblProfileUser> tblProfileUser { get; set; }
        public DbSet<tblEmployer> tblEmployer { get; set; }
        public DbSet<tblRole> tblRole { get; set; }
        public DbSet<tblJob> tblJob { get; set; }

        public DbSet<tblCompany> tblCompany { get; set; }

        public DbSet<tblReviews> tblReviews { get; set; }

        public DbSet<tblCategory> tblCategory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ProductModel>()
            //     .HasKey(p => p.ProductId);
            // base.OnModelCreating(modelBuilder);

        }

        public void registerAccount(int accountID, string username, string password, string emailaccount, int roleID)
        {

            var role = tblRole.FirstOrDefault(r => r.iRoleID == roleID);

            if (role == null)
            {
               throw new ArgumentException("Invalid RoleID, role not found.");
            }


            var newAccount = new tblAccount
            {
                iAccountID = accountID,
                sUserName = username,
                sPassword = password,
                sEmailAccount = emailaccount,
                iRoleID = roleID
            };

            tblAccount.Add(newAccount);

            SaveChanges();
        }
    }
}
