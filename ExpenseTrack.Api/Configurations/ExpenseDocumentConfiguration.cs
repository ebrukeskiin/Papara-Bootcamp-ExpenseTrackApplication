using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrack.Api.Configurations
{
    public class ExpenseDocumentConfiguration : IEntityTypeConfiguration<ExpenseDocument>
    {
        public void Configure(EntityTypeBuilder<ExpenseDocument> builder)
        {
            builder.ToTable("ExpenseDocuments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.FileType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.UploadedAt)
                   .HasDefaultValueSql("GETDATE()");

            // BaseEntity alanları
            builder.Property(x => x.InsertedDate)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // Navigation: Belge -> Masraf ilişkisi
            builder.HasOne(d => d.Expense)
                   .WithMany(e => e.Documents)
                   .HasForeignKey(d => d.ExpenseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
