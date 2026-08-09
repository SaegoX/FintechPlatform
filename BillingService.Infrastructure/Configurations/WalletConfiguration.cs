using BillingService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Infrastructure.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("wallets");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id).HasColumnName("id");

            builder.Property(w => w.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(w => w.Balance).HasColumnName("balance").HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(w => w.BlockedBalance).HasColumnName("blocked_balance").HasColumnName("numeric(18,2)").IsRequired();

            builder.Property(w => w.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();

            builder.Ignore(w => w.DomainEvents);
        }
    }
}
