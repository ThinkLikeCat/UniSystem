using MediatR;

namespace UniSystem.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<string>;
