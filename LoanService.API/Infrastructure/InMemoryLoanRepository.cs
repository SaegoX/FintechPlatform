using LoanService.Application.Common;
using LoanService.Domain.Aggregates;
using System.Collections.Concurrent;

namespace LoanService.API.Infrastructure
{
    public class InMemoryLoanRepository : ILoanRepository
    {
        private static readonly ConcurrentDictionary<Guid, LoanApplication> _loans = new();

        public Task AddAsync(LoanApplication loanApplication, CancellationToken cancellationToken)
        {
            _loans[loanApplication.Id] = loanApplication;
            return Task.CompletedTask;
        }

        public Task<LoanApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _loans.TryGetValue(id, out var loanApplication);
            return Task.FromResult(loanApplication);
        }
    }
}
