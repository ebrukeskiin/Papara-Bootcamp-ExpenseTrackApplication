using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrack.Api.Configurations
{

    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.ToTable("PaymentTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TransactionReference)
                   .HasMaxLength(100);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<int>(); // Enum olarak integer saklanacak

            builder.Property(x => x.CompletedAt)
                   .IsRequired(false);

            // BaseEntity alanları
            builder.Property(x => x.InsertedDate)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // One-to-One: PaymentTransaction <-> Expense
            builder.HasOne(p => p.Expense)
                   .WithOne(e => e.Payment)
                   .HasForeignKey<PaymentTransaction>(p => p.ExpenseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
