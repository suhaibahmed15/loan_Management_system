using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure.Data;

public static class SeedData
{
    public const string DefaultAdminRoleName = "Admin";
    public const string DefaultAdminRoleKey = "ADMIN";

    public static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        bool isDevelopment,
        CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync(cancellationToken);

        // Seed only the Admin role initially; Admin creates additional roles (Loan Officer, Customer, etc.) later.
        var adminRole = await context.Roles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Key == DefaultAdminRoleKey && !r.IsDeleted, cancellationToken);

        if (adminRole is null)
        {
            adminRole = new Role
            {
                Name = DefaultAdminRoleName,
                Key = DefaultAdminRoleKey,
                Description = "System administrator with full access to create roles, user profiles, and manage permissions.",
                IsActive = true
            };

            context.Roles.Add(adminRole);
            await context.SaveChangesAsync(cancellationToken);
        }

        // Seed only the initial Admin user credentials if no Admin user exists yet.
        var hasAdminUser = await context.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.RoleId == adminRole.Id && !u.IsDeleted, cancellationToken);

        if (hasAdminUser)
        {
            return;
        }

        var email = configuration["SeedAdmin:Email"];
        var fullName = configuration["SeedAdmin:FullName"];
        var phoneNumber = configuration["SeedAdmin:PhoneNumber"];
        var password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email))
        {
            email = isDevelopment
                ? "admin@lms.local"
                : throw new InvalidOperationException("SeedAdmin:Email must be configured outside Development.");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            fullName = "Suhaib";
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            password = isDevelopment
                ? "Admin@12345"
                : throw new InvalidOperationException("SeedAdmin:Password must be configured outside Development.");
        }

        var emailAlreadyExists = await context.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);

        if (emailAlreadyExists)
        {
            return;
        }

        var adminUser = new User
        {
            RoleId = adminRole.Id,
            FullName = fullName,
            Email = email,
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber,
            IsActive = true
        };

        var passwordHasher = new PasswordHasher<User>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, password);

        context.Users.Add(adminUser);
        await context.SaveChangesAsync(cancellationToken);
    }
}

