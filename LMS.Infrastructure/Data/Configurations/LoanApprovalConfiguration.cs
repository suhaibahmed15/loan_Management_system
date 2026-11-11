using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanApprovalConfiguration : IEntityTypeConfiguration<LoanApproval>
{
    public void Configure(EntityTypeBuilder<LoanApproval> builder)
    {
        builder.ToTable("LoanApprovals");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DecisionAt)
            .IsRequired();

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(x => x.LoanApplication)
            .WithMany(a => a.Approvals)
            .HasForeignKey(x => x.LoanApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DecisionByUser)
            .WithMany(u => u.LoanApprovals)
            .HasForeignKey(x => x.DecisionByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.LoanApprovals)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

