using LoanService.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanService.Application.UseCases.LoanApplications
{
    public class ApproveLoanApplicationCommandHandler : IRequestHandler<ApproveLoanApplicationCommand>
    {
        private readonly ILoanRepository _loanRepository;

        public ApproveLoanApplicationCommandHandler(ILoanRepository
            loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var loanApplication = await _loanRepository.GetByIdAsync(request.LoanApplicationId, cancellationToken) 
                ?? throw new KeyNotFoundException($"Заявка на займ с ID {request.LoanApplicationId} не найдена");

            loanApplication.Approve();

            await _loanRepository.UpdateAsync(loanApplication, cancellationToken);
        }
    }
}