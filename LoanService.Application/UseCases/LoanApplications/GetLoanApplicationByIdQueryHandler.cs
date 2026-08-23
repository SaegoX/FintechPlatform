using LoanService.Application.Common;
using LoanService.Application.UseCases.LoanApplications.DTOs;
using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public class GetLoanApplicationByIdQueryHandler : IRequestHandler<GetLoanApplicationByIdQuery, LoanApplicationDto>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICacheService _cacheService;

        public GetLoanApplicationByIdQueryHandler(ILoanRepository loanRepository, ICacheService cacheService)
        {
            _loanRepository = loanRepository;
            _cacheService = cacheService;
        }

        public async Task<LoanApplicationDto> Handle(GetLoanApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"loan_{request.LoanApplicationId}";

            var cachedDto = await _cacheService.GetAsync<LoanApplicationDto>(cacheKey, cancellationToken);
            if(cachedDto != null)
            {
                return cachedDto;
            }

            var loan = await _loanRepository.GetByIdAsync(request.LoanApplicationId, cancellationToken) 
                ?? throw new KeyNotFoundException($"Заявка на займ с ID {request.LoanApplicationId} не найдена");

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

            var loanDto = new LoanApplicationDto(
                loan.Id,
                loan.BorrowerId,
                loan.Amount,
                loan.InterestRate,
                loan.TermInMonths,
                loan.Status.ToString(),
                loan.CreatedAt,
                scheduleDtos
            );

            await _cacheService.SetAsync(cacheKey, loanDto, TimeSpan.FromMinutes(5), cancellationToken);

            return loanDto;
        }
    }
}