using LoanService.Application.UseCases.LoanApplications.DTOs;
using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public record GetLoanApplicationByIdQuery(Guid LoanApplicationId) : IRequest<LoanApplicationDto>;
}
