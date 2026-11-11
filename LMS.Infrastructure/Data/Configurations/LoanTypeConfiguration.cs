using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class LoanTypeConfiguration : IEntityTypeConfiguration<LoanType>
{
    public void Configure(EntityTypeBuilder<LoanType> builder)
    {
        builder.ToTable("LoanTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.MinAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.MaxAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.InterestRate)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

