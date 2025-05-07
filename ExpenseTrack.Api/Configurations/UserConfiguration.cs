using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrack.Api.Configurations
{
    public class UserConfiguration:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.PasswordHash)
                   .IsRequired();

            builder.Property(x => x.IBAN)
                   .HasMaxLength(34);

            builder.Property(x => x.Role)
                   .IsRequired()
                   .HasConversion<int>();

            // BaseEntity alanları (isteğe bağlı Fluent ayarları)
            builder.Property(x => x.InsertedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // Navigation
            builder.HasMany(u => u.Expenses)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

