using LoanService.Domain.Common;
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
        public int TermInMonth { get; private set; }
        public LoanStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public LoanApplication(Guid id, Guid borrowerId, decimal amount, decimal interestRate, int termInMonth)
            : base(id)
        {
            if (borrowerId == Guid.Empty)
                throw new ArgumentException("Идентификтор заёмщика обязателен.");
            if (amount <= 0)
                throw new ArgumentException("Сумма займа должна быть больше нуля.");
            if (interestRate <= 0 || interestRate > 1)
                throw new ArgumentException("Процентная ставка должна быть в диапазоне от 0 до 1");
            if (termInMonth <= 0)
                throw new ArgumentException("Срок займа должен быть больше нуля.");

            BorrowerId = borrowerId;
            Amount = amount;
            InterestRate = interestRate;
            TermInMonth = termInMonth;
            Status = LoanStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        protected LoanApplication(){ }

        public void Submit()
        {
            if (Status != LoanStatus.Draft)
                throw new InvalidOperationException("Отправить на рассмотрение можно только черновик.");

            Status = LoanStatus.Submitted;
        }

        public void Approve()
        {
            if (Status != LoanStatus.Submitted)
                throw new InvalidOperationException("Одобрить можно только заявку, находящуюся на рассмотрении.");

            Status = LoanStatus.Approved;
        }
    }
}
