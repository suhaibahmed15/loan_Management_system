using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("Statuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.Key)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => new { x.Category, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            // Application statuses
            new Status { Id = 1, Category = "Application", Name = "Draft", Key = "APPLICATION_DRAFT", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 2, Category = "Application", Name = "Submitted", Key = "APPLICATION_SUBMITTED", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 3, Category = "Application", Name = "UnderReview", Key = "APPLICATION_UNDER_REVIEW", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 4, Category = "Application", Name = "DocumentsRequired", Key = "APPLICATION_DOCUMENTS_REQUIRED", SortOrder = 4, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 5, Category = "Application", Name = "Approved", Key = "APPLICATION_APPROVED", SortOrder = 5, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 6, Category = "Application", Name = "Rejected", Key = "APPLICATION_REJECTED", SortOrder = 6, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 7, Category = "Application", Name = "Cancelled", Key = "APPLICATION_CANCELLED", SortOrder = 7, IsActive = true, CreatedAt = seedDate, IsDeleted = false },

            // Guarantor statuses
            new Status { Id = 8, Category = "Guarantor", Name = "Pending", Key = "GUARANTOR_PENDING", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 9, Category = "Guarantor", Name = "Verified", Key = "GUARANTOR_VERIFIED", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 10, Category = "Guarantor", Name = "Rejected", Key = "GUARANTOR_REJECTED", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },

            // Approval statuses
            new Status { Id = 11, Category = "Approval", Name = "Pending", Key = "APPROVAL_PENDING", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 12, Category = "Approval", Name = "Approved", Key = "APPROVAL_APPROVED", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 13, Category = "Approval", Name = "Rejected", Key = "APPROVAL_REJECTED", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },

            // Loan statuses
            new Status { Id = 14, Category = "Loan", Name = "Approved", Key = "LOAN_APPROVED", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 15, Category = "Loan", Name = "Disbursed", Key = "LOAN_DISBURSED", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 16, Category = "Loan", Name = "Active", Key = "LOAN_ACTIVE", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 17, Category = "Loan", Name = "Closed", Key = "LOAN_CLOSED", SortOrder = 4, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 18, Category = "Loan", Name = "Defaulted", Key = "LOAN_DEFAULTED", SortOrder = 5, IsActive = true, CreatedAt = seedDate, IsDeleted = false },

            // Repayment schedule statuses
            new Status { Id = 19, Category = "RepaymentSchedule", Name = "Pending", Key = "SCHEDULE_PENDING", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 20, Category = "RepaymentSchedule", Name = "PartiallyPaid", Key = "SCHEDULE_PARTIALLY_PAID", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 21, Category = "RepaymentSchedule", Name = "Paid", Key = "SCHEDULE_PAID", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 22, Category = "RepaymentSchedule", Name = "Overdue", Key = "SCHEDULE_OVERDUE", SortOrder = 4, IsActive = true, CreatedAt = seedDate, IsDeleted = false },

            // Repayment statuses
            new Status { Id = 23, Category = "Repayment", Name = "Pending", Key = "REPAYMENT_PENDING", SortOrder = 1, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 24, Category = "Repayment", Name = "Completed", Key = "REPAYMENT_COMPLETED", SortOrder = 2, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 25, Category = "Repayment", Name = "Failed", Key = "REPAYMENT_FAILED", SortOrder = 3, IsActive = true, CreatedAt = seedDate, IsDeleted = false },
            new Status { Id = 26, Category = "Repayment", Name = "Reversed", Key = "REPAYMENT_REVERSED", SortOrder = 4, IsActive = true, CreatedAt = seedDate, IsDeleted = false }
        );
    }
}

