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

        if (!context.Departments.Any())
        {
            context.Departments.AddRange(
                new Department("Факультет информационных технологий"),
                new Department("Факультет экономики и управления"),
                new Department("Факультет гуманитарных наук")
            );
            await context.SaveChangesAsync();
        }

        if (!context.Specialties.Any())
        {
            context.Specialties.AddRange(
                new Specialty("Программная инженерия", "09.03.04", 4),
                new Specialty("Прикладная информатика", "09.03.03", 4),
                new Specialty("Экономика", "38.03.01", 4)
            );
            await context.SaveChangesAsync();
        }

        if (!context.StudentStatuses.Any())
        {
            context.StudentStatuses.AddRange(
                new StudentStatus("Активный"),
                new StudentStatus("Академический отпуск"),
                new StudentStatus("Отчислен")
            );
            await context.SaveChangesAsync();
        }

        if (!context.AcademicGroups.Any())
        {
            context.AcademicGroups.AddRange(
                new AcademicGroup("ПИ-41", 30, 4, 1),
                new AcademicGroup("ПИ-31", 30, 3, 1),
                new AcademicGroup("ЭК-41", 25, 4, 3)
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

        var seedUsers = new List<(string Email, string Password, string FirstName, string LastName, string? Patronymic, SystemRoleName Role, string? Ticket, int? GroupId, int? StatusId, int? DeptId)>
        {
            ("student@unisystem.local", "Student123!", "Иван", "Петров", "Сергеевич", SystemRoleName.StudentProfile, "СТ-2022-001", 1, 1, null),
            ("staff@unisystem.local", "Staff123!", "Мария", "Иванова", "Алексеевна", SystemRoleName.StaffProfile, null, null, null, 1),
            ("dean@unisystem.local", "Dean123!", "Александр", "Смирнов", "Владимирович", SystemRoleName.Dean, null, null, null, 1),
            ("secretary@unisystem.local", "Secretary123!", "Елена", "Козлова", "Дмитриевна", SystemRoleName.Secretary, null, null, null, 2),
            ("curator@unisystem.local", "Curator123!", "Дмитрий", "Новиков", "Олегович", SystemRoleName.Curator, null, null, null, 2),
        };

        foreach (var (email, password, firstName, lastName, patronymic, role, ticket, groupId, statusId, deptId) in seedUsers)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
                continue;

            var user = new User(firstName, lastName, patronymic, Domain.Enums.Sex.Male, "default", email);
            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                throw new InvalidOperationException($"Failed to create {role} user: {string.Join("; ", createResult.Errors.Select(e => e.Description))}");

            await userManager.AddToRoleAsync(user, role.ToString());

            if (role == SystemRoleName.StudentProfile)
            {
                user.CreateStudentProfile(ticket!, groupId!.Value, statusId!.Value);
            }
            else
            {
                user.CreateStaffProfile(deptId!.Value, null);
            }

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new InvalidOperationException($"Failed to update {role} user: {string.Join("; ", updateResult.Errors.Select(e => e.Description))}");
        }
    }
}
