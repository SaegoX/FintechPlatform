using BillingService.Domain.Common;

namespace BillingService.Domain.Aggregates
{
    public class Wallet : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }


        public decimal BlockedBalance { get; private set; }
        public string Currency { get; private set; } = string.Empty;

        public Wallet(Guid id, Guid userId, string currency = "RUB") : base(id)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Идентификатор пользователя обязателен.");
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Валюта должна быть указана");

            UserId = userId;
            Balance = 0m;
            BlockedBalance = 0m;
            Currency = currency.ToUpper();
        }

        protected Wallet() { }


        public void Deposit(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Сумма пополнения должна быть больше нуля.");

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма списания должна быть больше нуля.");
            if (Balance < amount)
                throw new InvalidOperationException("Недостаточно средств на балансе кошелька.");

            Balance -= amount;
        }

        public void BlockFunds(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма блокировки должна быть больше нуля.");
            if (Balance < amount)
                throw new InvalidOperationException("Недостаточно свободных средств для разблокировки");

            Balance -= amount;
            BlockedBalance += amount;
        }

        public void UnbluckFunds(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма разблокировки должна быть больше нуля.");
            if (Balance < amount)
                throw new InvalidOperationException("Сумма разблокировки превышает заблакированный баланс.");

            BlockedBalance -= amount;
            Balance += amount;
        }

        public void ConfirmDebitingBlockedFunds(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма списания должна быть больше нуля.");
            if (BlockedBalance < amount)
                throw new InvalidOperationException("Недостаточно заблакированных средств для списания.");

            BlockedBalance -= amount;
        }
    }

}
