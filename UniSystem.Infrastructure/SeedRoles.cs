using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;

namespace UniSystem.Infrastructure;

public static class SeedRoles
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        var roles = new Dictionary<SystemRoleName, (string Nominative, string Dative)>
        {
            [SystemRoleName.StaffProfile]   = ("Сотрудник", "Сотрудника"),
            [SystemRoleName.StudentProfile] = ("Студент", "Студента"),
            [SystemRoleName.Dean]           = ("Декан", "Декана"),
            [SystemRoleName.Secretary]      = ("Секретарь", "Секретаря"),
            [SystemRoleName.Curator]        = ("Куратор", "Куратора"),
            [SystemRoleName.Admin]          = ("Администратор", "Администратора")
        };

        foreach (var (systemName, (nominative, dative)) in roles)
        {
            var roleName = systemName.ToString();

            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var role = new Role(systemName, nominative, dative);
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
            }
        }
    }
}
