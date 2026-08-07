using LoanService.Application.Common;
using LoanService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure.Repositories
{
    public class PostgresLoanRepository : ILoanRepository
    {
        private readonly LoanDbContext _context;

        public PostgresLoanRepository(LoanDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoanApplication loanApplication, CancellationToken cancellationToken)
        {
            await _context.LoanApplications.AddAsync(loanApplication, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<LoanApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.LoanApplications
                .Include(l => l.RepaymentSchedule)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(LoanApplication loanApplication, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
