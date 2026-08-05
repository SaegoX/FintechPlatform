using MediatR;

namespace LoanService.Application.UseCases.LoanApplications
{
    public record ApproveLoanApplicationCommand(Guid LoanApplicationId) : IRequest;
}