using LoanService.Domain.Aggregates;

namespace LoanService.Application.Common
{
    public interface ILoanRepository
    {
        Task AddAsync(LoanApplication loanApplication, CancellationToken cancellationToken);

        Task<LoanApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
