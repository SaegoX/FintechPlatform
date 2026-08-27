namespace LoanService.Application.Common
{
    public interface IBillingServiceChannel
    {
        Task<bool> IsWalletActiveAsync(Guid userId, CancellationToken cancellationToken);
    }
}
