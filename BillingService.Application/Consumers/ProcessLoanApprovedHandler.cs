using BillingService.Application.Common;
using BillingService.Domain.Aggregates;
using FintechPlatform.Contracts;

namespace BillingService.Application.Consumers
{
    public class ProcessLoanApprovedHandler
    {
        private readonly IWalletRepository _walletRepository;

        public ProcessLoanApprovedHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task HandleAsync(LoanApprovedIntegrationEvent message, CancellationToken cancellationToken)
        {
            var existingWallet = await _walletRepository.GetByUserIdAsync(message.BorrowerId, cancellationToken);

            if (existingWallet == null)
            {
                var newWallet = new Wallet(
                    id: Guid.NewGuid(),
                    userId: message.BorrowerId,
                    currency: "RUB"
                    );

                await _walletRepository.AddAsync(newWallet, cancellationToken);
            }
        }
    }
}