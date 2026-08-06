using LoanService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure
{
    public class LoanDbContext : DbContext
    {
        public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();

        public LoanDbContext(DbContextOptions<LoanDbContext> options) 
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanDbContext).Assembly);
        }
    }
}
