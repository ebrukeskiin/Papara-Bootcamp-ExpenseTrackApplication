using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrack.Api.Configurations
{
    public class ExpenseConfiguration
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
       .HasColumnType("decimal(18,2)");


            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.Property(x => x.Location)
                   .HasMaxLength(200);

            builder.Property(x => x.PaymentMethod)
                   .HasMaxLength(100);

            builder.Property(x => x.RejectionReason)
                   .HasMaxLength(500);

            builder.Property(x => x.ExpenseDate)
                   .IsRequired();

            builder.Property(x => x.ExpenseStatus)
                   .IsRequired()
                   .HasConversion<int>(); // Enum olarak maplenir

            // BaseEntity property’leri
            builder.Property(x => x.InsertedDate)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // User (talebi oluşturan)
            builder.HasOne(x => x.User)
                   .WithMany(u => u.Expenses)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProcessedByUser (admin onaylayan/işleyen kullanıcı)
            builder.HasOne(x => x.ProcessedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.ProcessedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ExpenseCategory
            builder.HasOne(x => x.ExpenseCategory)
                   .WithMany(c => c.Expenses)
                   .HasForeignKey(x => x.ExpenseCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Documents
            builder.HasMany(x => x.Documents)
                   .WithOne(d => d.Expense)
                   .HasForeignKey(d => d.ExpenseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Payment (bire bir ilişki)
            builder.HasOne(x => x.Payment)
                   .WithOne(p => p.Expense)
                   .HasForeignKey<PaymentTransaction>(p => p.ExpenseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
