namespace LoanService.Application.UseCases.LoanApplications.DTOs
{
    public record LoanApplicationDto(
        Guid Id,
        Guid BorrowerId,
        decimal Amount,
        decimal InterestRate,
        int TermInMonths,
        string Status,
        DateTime CreatedAt,
        IReadOnlyCollection<RepaymentPeriodDto> RepaymentSchedule
        );
}
