namespace LoanService.Application.Common
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken) where T : class;
    }
}
