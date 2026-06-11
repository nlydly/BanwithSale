using Microsoft.EntityFrameworkCore;
using BanwithSale.Models;
using AspNetCoreGeneratedDocument;

namespace BanwithSale.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserPlan> UserPlans { get; set; }

        //public DbSet<MyEarning> myEarnings {get; set; }

        public DbSet <Transaction> Transactions { get; set; }
    }
}