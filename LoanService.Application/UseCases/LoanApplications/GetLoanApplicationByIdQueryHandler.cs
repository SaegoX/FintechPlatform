using LoanService.Application.Common;
using LoanService.Application.UseCases.LoanApplications.DTOs;
using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public class GetLoanApplicationByIdQueryHandler : IRequestHandler<GetLoanApplicationByIdQuery, LoanApplicationDto>
    {
        private readonly ILoanRepository _loanRepository;

        public GetLoanApplicationByIdQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<LoanApplicationDto> Handle(GetLoanApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(request.LoanApplicationId, cancellationToken) ?? throw new KeyNotFoundException($"Заявка на займ с ID {request.LoanApplicationId} не найдена");

            var scheduleDtos = loan.RepaymentSchedule
                .Select(p => new RepaymentPeriodDto(
                    p.PeriodNumber,
                    p.DueDate,
                    p.MonthlyPayment,
                    p.InterestPayment,
                    p.PrincipalPayment,
                    p.RemainingPrincipal
                    ))
                .ToList();

            return new LoanApplicationDto(
                loan.Id,
                loan.BorrowerId,
                loan.Amount,
                loan.InterestRate,
                loan.TermInMonths,
                loan.Status.ToString(),
                loan.CreatedAt,
                scheduleDtos
            );
        }
    }
}
