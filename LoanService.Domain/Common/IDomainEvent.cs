namespace LoanService.Domain.Common
{
    public interface IDomainEvent
    {
        Guid EventId {  get; }
        DateTime Occurred { get; }
    }
}
