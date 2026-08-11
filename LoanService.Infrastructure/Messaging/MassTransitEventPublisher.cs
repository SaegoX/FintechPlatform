using LoanService.Application.Common;
using MassTransit;

namespace LoanService.Infrastructure.Messaging
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken) where T : class
        {
            await _publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}
