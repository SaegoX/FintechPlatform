using LoanService.Domain.Aggregates;

namespace LoanService.Application.Common
{
    public interface ILoanRepository
    {
        Task AddAsync(LoanApplication loanApplication, CancellationToken cancellationToken);
    }
}
