using LoanService.Domain.Common;

namespace LoanService.Domain.ValueObjects
{
    public class RepaymentPeriod : ValueObject
    {
        public int PeriodNumber { get; private set; }
        public DateTime DueDate { get; private set; }
        public decimal MonthlyPayment {  get; private set; }
        public decimal InterestPayment { get; private set; }
        public decimal PrincipalPayment { get; private set; }
        public decimal RemainingPrincipal {  get; private set; }

        public RepaymentPeriod(int periodNumber, DateTime dueDate, decimal monthlyPayment, decimal interestPayment, decimal principalPayment, decimal remainingPrincipal)
        {
            PeriodNumber = periodNumber;
            DueDate = dueDate;
            MonthlyPayment = monthlyPayment;
            InterestPayment = interestPayment;
            PrincipalPayment = principalPayment;
            RemainingPrincipal = remainingPrincipal;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PeriodNumber;
            yield return DueDate;
            yield return MonthlyPayment;
            yield return InterestPayment;
            yield return PrincipalPayment;
            yield return RemainingPrincipal;
        }
    }
}
