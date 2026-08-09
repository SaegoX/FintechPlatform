using BillingService.Application.Common;
using BillingService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Repositories
{
    public class PostgresWalletRepository : IWalletRepository
    {
        private readonly BillingDbContext _context;

        public PostgresWalletRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            await _context.Wallets.AddAsync(wallet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
        }

        public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
