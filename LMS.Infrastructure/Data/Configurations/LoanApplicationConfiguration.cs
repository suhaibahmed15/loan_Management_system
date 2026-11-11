using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
{
    public void Configure(EntityTypeBuilder<LoanApplication> builder)
    {
        builder.ToTable("LoanApplications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApplicationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.RequestedAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Purpose)
            .HasMaxLength(1000);

        builder.Property(x => x.ReviewRemarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.ApplicationNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.ApplicantUser)
            .WithMany(u => u.LoanApplications)
            .HasForeignKey(x => x.ApplicantUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReviewedByUser)
            .WithMany(u => u.ReviewedLoanApplications)
            .HasForeignKey(x => x.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LoanType)
            .WithMany(lt => lt.LoanApplications)
            .HasForeignKey(x => x.LoanTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.LoanApplications)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

