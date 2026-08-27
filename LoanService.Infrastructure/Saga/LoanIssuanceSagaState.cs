using MassTransit;

namespace LoanService.Infrastructure.Saga
{
    public class LoanIssuanceSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get ; set; }
        public string CurrentState { get; set; } = string.Empty;

        public Guid BorrowerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime? UpdatedAt {  get; set; }
    }
}
