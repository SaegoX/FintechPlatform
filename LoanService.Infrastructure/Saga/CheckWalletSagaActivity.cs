using FintechPlatform.Contracts;
using LoanService.Application.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LoanService.Infrastructure.Saga
{
    public class CheckWalletSagaActivity : IStateMachineActivity<LoanIssuanceSagaState, LoanApprovedIntegrationEvent>
    {
        private readonly IBillingServiceChannel _billingChannel;
        private readonly ILogger<CheckWalletSagaActivity> _logger;

        public CheckWalletSagaActivity(IBillingServiceChannel billingChannel, ILogger<CheckWalletSagaActivity> logger)
        {
            _billingChannel = billingChannel;
            _logger = logger;
        }

        public void Probe(ProbeContext context) => context.CreateScope("check-wallet-activity");

        public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

        public async Task Execute(BehaviorContext<LoanIssuanceSagaState, LoanApprovedIntegrationEvent> context, IBehavior<LoanIssuanceSagaState, LoanApprovedIntegrationEvent> next)
        {
            _logger.LogInformation("Сага делает gRPC-запрос в Билинг для проверки кошелька заёмщика {BorrowerId}", context.Saga.BorrowerId);

            bool isWalletActive = await _billingChannel.IsWalletActiveAsync(context.Saga.BorrowerId, context.CancellationToken);

            if (isWalletActive)
            {
                _logger.LogInformation("gRPC подтвердил: кошелёк заёмщика активен! Транзакция Саги завершена успешно");

                context.Saga.UpdatedAt = DateTime.UtcNow;
                context.Saga.CurrentState = "Completed";
            }
            else
            {
                _logger.LogWarning("gRPC отклонил: у заёмщика {BorrowerId} нет кошелька! Сага переходит в статус ошибки", context.Saga.BorrowerId);
                context.Saga.UpdatedAt = DateTime.UtcNow;
                context.Saga.CurrentState = "Failed";
            }

            await next.Execute(context);
        }
        
        public Task Faulted<TException>(
            BehaviorExceptionContext<LoanIssuanceSagaState, LoanApprovedIntegrationEvent, TException> context, 
            IBehavior<LoanIssuanceSagaState, LoanApprovedIntegrationEvent> next) 
            where TException : Exception
        {
            _logger.LogError(context.Exception, "Внутри активности Саги произошёл критический сбой!");
            return next.Faulted(context);
        }
    }
}
