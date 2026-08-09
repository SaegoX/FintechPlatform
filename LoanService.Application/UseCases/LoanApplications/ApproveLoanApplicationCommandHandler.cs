using FintechPlatform.Contracts;
using LoanService.Application.Common;
using MassTransit;
using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public class ApproveLoanApplicationCommandHandler : IRequestHandler<ApproveLoanApplicationCommand>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ApproveLoanApplicationCommandHandler(ILoanRepository loanRepository, IPublishEndpoint publishEndpoint)
        {
            _loanRepository = loanRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var loanApplication = await _loanRepository.GetByIdAsync(request.LoanApplicationId, cancellationToken) 
                ?? throw new KeyNotFoundException($"Заявка на займ с ID {request.LoanApplicationId} не найдена");

            loanApplication.Approve();

            await _loanRepository.UpdateAsync(loanApplication, cancellationToken);

            var integrationEvent = new LoanApprovedIntegrationEvent(
                loanApplicationId: loanApplication.Id,
                borrowerId: loanApplication.BorrowerId,
                amount: loanApplication.Amount,
                interestRate: loanApplication.InterestRate,
                termInMonths: loanApplication.TermInMonths
                );

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}