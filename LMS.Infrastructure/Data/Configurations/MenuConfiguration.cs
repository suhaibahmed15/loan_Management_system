using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Route)
            .HasMaxLength(250);

        builder.Property(x => x.Icon)
            .HasMaxLength(100);

        builder.Property(x => x.DisplayOrder)
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

        builder.HasOne(x => x.ParentMenu)
            .WithMany(m => m.ChildMenus)
            .HasForeignKey(x => x.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

