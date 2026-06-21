using MediatR;
using Microsoft.AspNetCore.Identity;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.FirstName, request.LastName, request.Patronymic, request.Sex, request.IconPath ?? "default", request.Email);

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to create user: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
        if (!roleResult.Succeeded)
        {
            var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to assign role: {errors}");
        }

        switch (request.Role)
        {
            case SystemRoleName.StudentProfile:
            {
                if (request.StudentTicket is null || request.AcademicGroupId is null || request.StudentStatusId is null)
                    throw new DomainException("Student profile requires: StudentTicket, AcademicGroupId, StudentStatusId.");

                user.CreateStudentProfile(request.StudentTicket, request.AcademicGroupId.Value, request.StudentStatusId.Value);
                break;
            }
            case SystemRoleName.StaffProfile:
            case SystemRoleName.Dean:
            case SystemRoleName.Secretary:
            case SystemRoleName.Curator:
            {
                if (request.DepartmentId is null)
                    throw new DomainException("Staff profile requires DepartmentId.");

                user.CreateStaffProfile(request.DepartmentId.Value, request.AcademicGroupId);
                break;
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to save profile: {errors}");
        }

        return user.Id;
    }
}
