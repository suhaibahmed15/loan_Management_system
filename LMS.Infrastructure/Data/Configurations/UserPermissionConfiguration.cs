using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("UserPermissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignedAt)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => new { x.UserId, x.PermissionId })
            .IsUnique()
            .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0");

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Permission)
            .WithMany(p => p.UserPermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany(u => u.AssignedUserPermissions)
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RevokedByUser)
            .WithMany(u => u.RevokedUserPermissions)
            .HasForeignKey(x => x.RevokedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

