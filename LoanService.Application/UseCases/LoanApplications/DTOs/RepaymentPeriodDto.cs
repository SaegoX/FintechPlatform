namespace LoanService.Application.UseCases.LoanApplications.DTOs
{
    public record RepaymentPeriodDto(
        int PeriodNumber,
        DateTime DueDate,
        decimal MonthlyPayment,
        decimal InterestPayment,
        decimal PrincipalPayment,
        decimal RemainingPrincipal
        );
}
