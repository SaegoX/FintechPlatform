using LoanService.Domain.Common;
using LoanService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoanService.Domain.Aggregates
{
    public class LoanApplication : AggregateRoot
    {
        public Guid BorrowerId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal InterestRate { get; private set; }
        public int TermInMonths { get; private set; }
        public LoanStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public LoanApplication(Guid id, Guid borrowerId, decimal amount, decimal interestRate, int termInMonths)
            : base(id)
        {
            if (borrowerId == Guid.Empty)
                throw new ArgumentException("Идентификтор заёмщика обязателен.");
            if (amount <= 0)
                throw new ArgumentException("Сумма займа должна быть больше нуля.");
            if (interestRate <= 0 || interestRate > 1)
                throw new ArgumentException("Процентная ставка должна быть в диапазоне от 0 до 1");
            if (termInMonths <= 0)
                throw new ArgumentException("Срок займа должен быть больше нуля.");

            BorrowerId = borrowerId;
            Amount = amount;
            InterestRate = interestRate;
            TermInMonths = termInMonths;
            Status = LoanStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        protected LoanApplication() { }

        public void Submit()
        {
            if (Status != LoanStatus.Draft)
                throw new InvalidOperationException("Отправить на рассмотрение можно только черновик.");

            Status = LoanStatus.Submitted;
        }

        private readonly List<RepaymentPeriod> _repaymentSchedule = new();
        public IReadOnlyCollection<RepaymentPeriod> RepaymentSchedule => _repaymentSchedule.AsReadOnly();

        public void Approve()
        {
            if (Status != LoanStatus.Submitted)
                throw new InvalidOperationException("Одобрить можно только заявку, находящуюся на рассмотрении.");

            Status = LoanStatus.Approved;
            CalculateRepaymentSchedule();
        }

        private void CalculateRepaymentSchedule()
        {
            _repaymentSchedule.Clear();

            double monthlyRate = (double)InterestRate / 12;
            double amount = (double)Amount;

            double annuityCoefficient = (monthlyRate * Math.Pow(1 + monthlyRate, TermInMonths)) /
                (Math.Pow(1 + monthlyRate, TermInMonths) - 1);

            decimal monthlyPayment = Math.Round((decimal)(amount * annuityCoefficient), 2);
            decimal remainingPrincipal = Amount;
            DateTime paymentDate = DateTime.UtcNow;

            for (int i = 1; i <= TermInMonths; i++)
            {
                paymentDate = paymentDate.AddMonths(1);

                decimal interestPayment = Math.Round(remainingPrincipal * (decimal)monthlyRate, 2);

                decimal principalPayment = monthlyPayment - interestPayment;

                if (i == TermInMonths)
                {
                    principalPayment = remainingPrincipal;
                    monthlyPayment = principalPayment + interestPayment;
                    remainingPrincipal = 0;
                }
                else
                {
                    remainingPrincipal -= principalPayment;
                }

                _repaymentSchedule.Add(new RepaymentPeriod(
                    periodNumber: i,
                    dueDate: paymentDate,
                    monthlyPayment: monthlyPayment,
                    interestPayment: interestPayment,
                    principalPayment: principalPayment,
                    remainingPrincipal: remainingPrincipal
                   ));
            }

        }
    }
}
