using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _userContext;

    public GetCurrentUserQueryHandler(UserManager<User> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
    }

    public async Task<CurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (userId is null)
            throw new DomainException("User is not authenticated.");

        var user = await _userManager.Users
            .Include(u => u.StudentProfile!).ThenInclude(p => p.AcademicGroup)
            .Include(u => u.StudentProfile!).ThenInclude(p => p.StudentStatus)
            .Include(u => u.StaffProfile!).ThenInclude(p => p.Department)
            .Include(u => u.StaffProfile!).ThenInclude(p => p.AcademicGroup)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new DomainException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Unknown";

        return new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName.Value,
            LastName: user.LastName.Value,
            Patronymic: user.Patronymic?.Value,
            FullName: user.FullName,
            Sex: user.Sex.ToString(),
            Role: role,
            IconPath: user.IconPath?.Value,
            StudentProfile: user.StudentProfile is not null
                ? new StudentProfileResponse(
                    StudentTicket: user.StudentProfile.StudentTicket,
                    AcademicGroupId: user.StudentProfile.AcademicGroupId,
                    AcademicGroupName: user.StudentProfile.AcademicGroup?.Name.Value,
                    StudentStatusId: user.StudentProfile.StudentStatusId,
                    StudentStatusName: user.StudentProfile.StudentStatus?.Name.Value
                )
                : null,
            StaffProfile: user.StaffProfile is not null
                ? new StaffProfileResponse(
                    DepartmentId: user.StaffProfile.DepartmentId,
                    DepartmentName: user.StaffProfile.Department?.Name.Value,
                    AcademicGroupId: user.StaffProfile.AcademicGroupId,
                    AcademicGroupName: user.StaffProfile.AcademicGroup?.Name.Value
                )
                : null
        );
    }
}
