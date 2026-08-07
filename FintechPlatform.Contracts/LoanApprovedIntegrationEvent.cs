namespace FintechPlatform.Contracts
{
    public record LoanApprovedIntegrationEvent
    {
        public Guid LoanApplicationId { get; init; }
        public Guid BorrowerId { get; init; }
        public decimal Amount { get; init; }
        public decimal InterestRate { get; init; }
        public int TermInMonths { get; init; }
        public DateTime ApprovedAt { get; init; }

        public LoanApprovedIntegrationEvent(Guid loanApplicationId, Guid borrowerId, decimal amount, decimal interestRate, int termInMonths)
        {
            LoanApplicationId = loanApplicationId;
            BorrowerId = borrowerId;
            Amount = amount;
            InterestRate = interestRate;
            TermInMonths = termInMonths;
            ApprovedAt = DateTime.UtcNow;
        }
    }
}
