using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrack.Api.Configurations
{
    public class ExpenseCategoryConfiguration:IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.ToTable("ExpenseCategories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(300);

            // BaseEntity alanları
            builder.Property(x => x.InsertedDate)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // Navigation: 1 kategori -> n masraf
            builder.HasMany(c => c.Expenses)
                   .WithOne(e => e.ExpenseCategory)
                   .HasForeignKey(e => e.ExpenseCategoryId)
                   .OnDelete(DeleteBehavior.Restrict); // Silinmesin, çünkü hala bağlı harcamalar olabilir
        }
    }
}
