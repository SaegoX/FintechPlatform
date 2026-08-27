using FintechPlatform.Contracts;
using LoanService.Application.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LoanService.Infrastructure.Saga
{
    public class LoanIssuanceStateMachine : MassTransitStateMachine<LoanIssuanceSagaState>
    {
        private readonly ILogger<LoanIssuanceStateMachine> _logger;
        private readonly IBillingServiceChannel _billingChannel;

        public Event<LoanApprovedIntegrationEvent> LoanApproved { get; private set; }

        public State CheckingWallet { get; private set; }
        public State Completed { get; private set; }
        public State Field { get; private set; }
        public LoanIssuanceStateMachine(IBillingServiceChannel billingChannel, ILogger<LoanIssuanceStateMachine> logger)
        {
            _billingChannel = billingChannel;
            _logger = logger;

            InstanceState(x => x.CurrentState);

            Event(() => LoanApproved, x => x.CorrelateById(context => context.Message.LoanApplicationId));

            During(Initial,
                When(LoanApproved)
                .Then(context =>
                {
                    _logger.LogInformation("Сага [Выдача Займа] запущена для Лоана {LoanId}", context.Message.LoanApplicationId);

                    context.Saga.BorrowerId = context.Message.BorrowerId;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.CreatedAt = DateTime.UtcNow;
                })
                .TransitionTo(CheckingWallet)
                .Activity(x => x.OfType<CheckWalletSagaActivity>())
                );
        }
    }
}
