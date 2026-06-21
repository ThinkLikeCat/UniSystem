using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;

namespace UniSystem.Infrastructure;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;
        var context = sp.GetRequiredService<UniSystemDbContext>();

        if (!context.DocumentStatuses.Any())
        {
            context.DocumentStatuses.AddRange(
                new DocumentStatus("Черновик"),
                new DocumentStatus("На проверке секретаря"),
                new DocumentStatus("На доработку"),
                new DocumentStatus("На проверке декана"),
                new DocumentStatus("Утверждён"),
                new DocumentStatus("Отклонён")
            );
            await context.SaveChangesAsync();
        }

        var userManager = sp.GetRequiredService<UserManager<User>>();
        var roleManager = sp.GetRequiredService<RoleManager<Role>>();

        if (!await roleManager.RoleExistsAsync(SystemRoleName.Admin.ToString()))
        {
            var adminRole = new Role(SystemRoleName.Admin, "Администратор", "Администратора");
            await roleManager.CreateAsync(adminRole);
        }

        if (await userManager.FindByEmailAsync("admin@unisystem.local") is null)
        {
            var admin = new User("Admin", "Admin", null, Domain.Enums.Sex.Male, "default", "admin@unisystem.local");
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create admin user.");

            await userManager.AddToRoleAsync(admin, SystemRoleName.Admin.ToString());
        }
    }
}
