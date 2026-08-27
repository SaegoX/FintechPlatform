using BillingService.Domain.Aggregates;

namespace BillingService.Domain.Tests
{
    public class WalletTests
    {
        [Fact]
        public void Deposit_WithValidInput_BalanceIncreases()
        {
            var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), "RUB");
            decimal depositAmount = 5000m;

            wallet.Deposit(depositAmount);

            Assert.Equal(depositAmount, wallet.Balance);
            Assert.Equal(0m, wallet.BlockedBalance);
        }

        [Fact]
        public void BlockFunds_BalanceIsSufficient_MovesMoneyToBlocked()
        {
            var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), "RUB");
            wallet.Deposit(10000m);

            wallet.BlockFunds(3000m);

            Assert.Equal(7000m, wallet.Balance);
            Assert.Equal(3000m, wallet.BlockedBalance);
        }

        [Fact]
        public void BlockFunds_BalanceIsInsufficient_ThrowInvalidOperationException()
        {
            var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), "RUB");
            wallet.Deposit(2000m);

            Assert.Throws<InvalidOperationException>(() => wallet.BlockFunds(5000m));
        }

        [Fact]
        public void Withdraw_AmountExceedsBalance_ShouldThrowInvalidOperationException()
        {
            var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), "RUB");
            wallet.Deposit(1000m);

            Assert.Throws<InvalidOperationException>(() => wallet.Withdraw(1500m));
        }
    }
}
