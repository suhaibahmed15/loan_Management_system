using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class RepaymentScheduleConfiguration : IEntityTypeConfiguration<RepaymentSchedule>
{
    public void Configure(EntityTypeBuilder<RepaymentSchedule> builder)
    {
        builder.ToTable("RepaymentSchedules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DueDate)
            .IsRequired();

        builder.Property(x => x.PrincipalDue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.InterestDue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalDue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PaidAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => new { x.LoanId, x.InstallmentNumber })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.Loan)
            .WithMany(l => l.RepaymentSchedules)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.RepaymentSchedules)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

