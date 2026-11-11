using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.ToTable("RoleMenus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => new { x.RoleId, x.MenuId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.Role)
            .WithMany(r => r.RoleMenus)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Menu)
            .WithMany(m => m.RoleMenus)
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

