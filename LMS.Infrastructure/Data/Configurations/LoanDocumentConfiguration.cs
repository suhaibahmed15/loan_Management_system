using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanDocumentConfiguration : IEntityTypeConfiguration<LoanDocument>
{
    public void Configure(EntityTypeBuilder<LoanDocument> builder)
    {
        builder.ToTable("LoanDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.VerificationRemarks)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(x => x.LoanApplication)
            .WithMany(a => a.Documents)
            .HasForeignKey(x => x.LoanApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UploadedByUser)
            .WithMany(u => u.UploadedLoanDocuments)
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VerifiedByUser)
            .WithMany(u => u.VerifiedLoanDocuments)
            .HasForeignKey(x => x.VerifiedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

