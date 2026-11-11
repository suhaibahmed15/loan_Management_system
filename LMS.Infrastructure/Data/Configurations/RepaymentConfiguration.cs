using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class RepaymentConfiguration : IEntityTypeConfiguration<Repayment>
{
    public void Configure(EntityTypeBuilder<Repayment> builder)
    {
        builder.ToTable("Repayments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PrincipalPaid)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.InterestPaid)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PaidAt)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.PaymentReference)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.Loan)
            .WithMany(l => l.Repayments)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RepaymentSchedule)
            .WithMany(rs => rs.Repayments)
            .HasForeignKey(x => x.RepaymentScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.Repayments)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RecordedByUser)
            .WithMany(u => u.RecordedRepayments)
            .HasForeignKey(x => x.RecordedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

