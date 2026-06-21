using MediatR;
using Microsoft.AspNetCore.Identity;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(UserManager<User> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new DomainException("Invalid email or password.");

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new DomainException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        return _jwtProvider.GenerateToken(user, roles);
    }
}
