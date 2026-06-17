using Microsoft.EntityFrameworkCore;
using BanwithSale.Models;

namespace BanwithSale.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Add all your DbSets here
        public DbSet<User> Users { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }     // ← This was missing
        public DbSet<Transaction> Transactions { get; set; }
    }
}