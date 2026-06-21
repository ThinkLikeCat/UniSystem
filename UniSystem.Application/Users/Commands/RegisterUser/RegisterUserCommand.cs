using MediatR;
using UniSystem.Domain.Enums;

namespace UniSystem.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Patronymic,
    Sex Sex,
    SystemRoleName Role,
    string? IconPath = null,
    string? StudentTicket = null,
    int? AcademicGroupId = null,
    int? StudentStatusId = null,
    int? DepartmentId = null
) : IRequest<Guid>;
