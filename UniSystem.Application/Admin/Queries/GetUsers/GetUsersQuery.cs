using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSystem.Domain.Entities;

namespace UniSystem.Application.Admin.Queries.GetUsers;

public record UserListItemDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    string FullName,
    string Role
);

public record GetUsersQuery(string? RoleFilter = null) : IRequest<List<UserListItemDto>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserListItemDto>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .OrderBy(u => u.LastName)
            .ToListAsync(cancellationToken);

        var result = new List<UserListItemDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Unknown";

            if (request.RoleFilter is null || role == request.RoleFilter)
            {
                result.Add(new UserListItemDto(
                    user.Id,
                    user.Email!,
                    user.FirstName.Value,
                    user.LastName.Value,
                    user.Patronymic?.Value,
                    user.FullName,
                    role
                ));
            }
        }

        return result;
    }
}
