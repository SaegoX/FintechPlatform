using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public record CreateLoanApplicationCommand(
        Guid BorrowerId,
        decimal Amount,
        decimal InterestRate,
        int TermInMonths
        ) : IRequest<Guid>;
}
