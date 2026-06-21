using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string? FirstName = null,
    string? LastName = null,
    string? Patronymic = null,
    Sex? Sex = null,
    SystemRoleName? Role = null
) : IRequest;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.StudentProfile)
            .Include(u => u.StaffProfile)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new DomainException("User not found.");

        if (request.FirstName is not null)
            user.SetFirstName(request.FirstName);

        if (request.LastName is not null)
            user.SetLastName(request.LastName);

        if (request.Patronymic is not null)
            user.SetPatronymic(request.Patronymic);

        if (request.Sex is not null)
            user.SetSex(request.Sex.Value);

        if (request.Role is not null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var targetRole = request.Role.Value;
            await _userManager.AddToRoleAsync(user, targetRole.ToString());

            if (targetRole == SystemRoleName.Admin)
            {
                if (user.StudentProfile is not null)
                    _context.StudentProfiles.Remove(user.StudentProfile);
                if (user.StaffProfile is not null)
                    _context.StaffProfiles.Remove(user.StaffProfile);
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to update user: {errors}");
        }
    }
}
