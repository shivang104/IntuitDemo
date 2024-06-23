using FinDataWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinDataWebAPI.Data
{
    public class FinancialDataContext : DbContext
    {
        public FinancialDataContext(DbContextOptions<FinancialDataContext> options) : base(options)
        {
        }

        public DbSet<FinancialData> FinancialData { get; set; }
        public DbSet<FinMetrics> FinMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinancialData>()
                .HasMany(f => f.finMetrics)
                .WithOne(m => m.financialData)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
