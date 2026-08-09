using BillingService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure
{
    public class BillingDbContext : DbContext
    {
        public DbSet<Wallet> Wallets => Set<Wallet>();

        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
        }
    }
}
