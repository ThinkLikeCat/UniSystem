using MediatR;

namespace UniSystem.Application.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<CurrentUserResponse>;
