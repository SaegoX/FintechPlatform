using LoanService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanService.Infrastructure.Configurations
{
    public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
    {
        public void Configure(EntityTypeBuilder<LoanApplication> builder)
        {
            builder.ToTable("loan_applications");

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).HasColumnName("id");

            builder.Property(l => l.BorrowerId)
                .HasColumnName("borrower_id")
                .IsRequired();

            builder.Property(l => l.Amount)
                .HasColumnName("amount")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(l => l.InterestRate)
                .HasColumnName("interest_rate")
                .HasColumnType("numeric(5,4)")
                .IsRequired();

            builder.Property(l => l.TermInMonths)
                .HasColumnName("term_in_months")
                .IsRequired();

            builder.Property(l => l.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(l => l.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.OwnsMany(l => l.RepaymentSchedule, sb =>
            {
                sb.ToTable("repayment_periods");
                sb.WithOwner().HasForeignKey("loan_application_id");
                sb.HasKey("loan_application_id", "PeriodNumber");

                sb.Property(p => p.PeriodNumber)
                .HasColumnName("period_number");

                sb.Property(p => p.DueDate)
                .HasColumnName("due_date");

                sb.Property(p => p.PeriodNumber)
                .HasColumnName("monthly_payment")
                .HasColumnType("numeric(18,2)");

                sb.Property(p => p.PeriodNumber)
                .HasColumnName("interest_payment")
                .HasColumnType("numeric(18,2)");

                sb.Property(p => p.PeriodNumber)
                .HasColumnName("principal_payment")
                .HasColumnType("numeric(18,2)");

                sb.Property(p => p.PeriodNumber)
                .HasColumnName("remaining_principal")
                .HasColumnType("numeric(18,2)");
            });

            builder.Ignore(l => l.DomainEvents);

        }
    }
}