using BillingService.Domain.Aggregates;

namespace BillingService.Application.Common
{
    public interface IWalletRepository
    {
        Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken);
    }
}
