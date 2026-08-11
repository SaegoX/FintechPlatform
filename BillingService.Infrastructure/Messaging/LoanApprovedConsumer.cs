using BillingService.Application.Consumers;
using FintechPlatform.Contracts;
using MassTransit;

namespace BillingService.Infrastructure.Messaging
{
    public class LoanApprovedConsumer : IConsumer<LoanApprovedIntegrationEvent>
    {
        private readonly ProcessLoanApprovedHandler _handler;

        public LoanApprovedConsumer(ProcessLoanApprovedHandler handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<LoanApprovedIntegrationEvent> context)
        {
            await _handler.HandleAsync(context.Message, context.CancellationToken);
        }
    }
}
