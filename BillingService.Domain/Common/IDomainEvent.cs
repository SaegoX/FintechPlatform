namespace BillingService.Domain.Common
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccuredOn { get; }
    }
}
