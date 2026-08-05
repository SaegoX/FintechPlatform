using LoanService.Application.Common;
using LoanService.Domain.Aggregates;
using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public class CreateLoanApplicationCommandHandler : IRequestHandler<CreateLoanApplicationCommand, Guid>
    {
        private readonly ILoanRepository _loanRepository;

        public CreateLoanApplicationCommandHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<Guid> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var loanId = Guid.NewGuid();

            var loanApplication = new LoanApplication(
                id: loanId,
                borrowerId: request.BorrowerId,
                amount: request.Amount,
                interestRate: request.InterestRate,
                termInMonths: request.TermInMonths
                );

            loanApplication.Submit();

            await _loanRepository.AddAsync(loanApplication, cancellationToken);

            return loanId;
        }
    }

}