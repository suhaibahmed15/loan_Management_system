using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanGuarantorConfiguration : IEntityTypeConfiguration<LoanGuarantor>
{
    public void Configure(EntityTypeBuilder<LoanGuarantor> builder)
    {
        builder.ToTable("LoanGuarantors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.NationalIdNumber)
            .HasMaxLength(50);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.RelationshipToApplicant)
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.VerificationRemarks)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(x => x.LoanApplication)
            .WithMany(a => a.Guarantors)
            .HasForeignKey(x => x.LoanApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.LoanGuarantors)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VerifiedByUser)
            .WithMany(u => u.VerifiedGuarantors)
            .HasForeignKey(x => x.VerifiedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

