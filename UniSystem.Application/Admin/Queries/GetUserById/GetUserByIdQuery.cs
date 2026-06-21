using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Queries.GetUserById;

public record UserDetailDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    string FullName,
    string Sex,
    string Role,
    string? StudentTicket,
    int? AcademicGroupId,
    string? AcademicGroupName,
    int? StudentStatusId,
    string? StudentStatusName,
    int? DepartmentId,
    string? DepartmentName,
    int? CuratorGroupId,
    string? CuratorGroupName
);

public record GetUserByIdQuery(Guid Id) : IRequest<UserDetailDto>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailDto>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.StudentProfile!).ThenInclude(p => p.AcademicGroup).ThenInclude(g => g!.Specialty)
            .Include(u => u.StudentProfile!).ThenInclude(p => p.StudentStatus)
            .Include(u => u.StaffProfile!).ThenInclude(p => p.Department)
            .Include(u => u.StaffProfile!).ThenInclude(p => p.AcademicGroup)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            throw new DomainException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Unknown";

        return new UserDetailDto(
            user.Id,
            user.Email!,
            user.FirstName.Value,
            user.LastName.Value,
            user.Patronymic?.Value,
            user.FullName,
            user.Sex.ToString(),
            role,
            user.StudentProfile?.StudentTicket,
            user.StudentProfile?.AcademicGroupId,
            user.StudentProfile?.AcademicGroup?.Name.Value,
            user.StudentProfile?.StudentStatusId,
            user.StudentProfile?.StudentStatus?.Name.Value,
            user.StaffProfile?.DepartmentId,
            user.StaffProfile?.Department?.Name.Value,
            user.StaffProfile?.AcademicGroupId,
            user.StaffProfile?.AcademicGroup?.Name.Value
        );
    }
}
