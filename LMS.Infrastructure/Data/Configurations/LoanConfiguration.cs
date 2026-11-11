using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LoanNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PrincipalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.InterestRate)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.TotalPayableAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.OutstandingBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.LoanNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => x.LoanApplicationId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.LoanApplication)
            .WithOne(a => a.Loan)
            .HasForeignKey<Loan>(x => x.LoanApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BorrowerUser)
            .WithMany(u => u.BorrowerLoans)
            .HasForeignKey(x => x.BorrowerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DisbursedByUser)
            .WithMany(u => u.DisbursedLoans)
            .HasForeignKey(x => x.DisbursedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.Loans)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

