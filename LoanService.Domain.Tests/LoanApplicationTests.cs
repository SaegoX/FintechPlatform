using LoanService.Domain.Aggregates;

namespace LoanService.Domain.Tests
{
    public class LoanApplicationTests
    {
        [Fact]
        public void Approve_SubmittedLoan_AnnuityCorrect()
        {
            var loanId = Guid.NewGuid();
            var borrowerId = Guid.NewGuid();
            decimal amount = 100000m;
            decimal interestRate = 0.12m;
            int terminMonth = 12;

            var loan = new LoanApplication(loanId, borrowerId, amount, interestRate, terminMonth);

            loan.Submit();

            loan.Approve();

            Assert.Equal(LoanStatus.Approved, loan.Status);

            Assert.Equal(terminMonth, loan.RepaymentSchedule.Count);

            var firstPeriod = loan.RepaymentSchedule.First();
            Assert.Equal(8884.88m, firstPeriod.MonthlyPayment);

            decimal totalPrincipalPaid = loan.RepaymentSchedule.Sum(p => p.PrincipalPayment);
            Assert.Equal(amount, totalPrincipalPaid);

            var lastPeriod = loan.RepaymentSchedule.Last();
            Assert.Equal(0m, lastPeriod.RemainingPrincipal);
        }

        [Fact]
        public void NegativeAmount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new LoanApplication(Guid.NewGuid(), Guid.NewGuid(), -500m, 0.15m, 12));
        }
    }
}
