using MediatR;
using Microsoft.AspNetCore.Identity;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(string CurrentPassword, string NewPassword) : IRequest;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserContext _userContext;

    public ChangePasswordCommandHandler(UserManager<User> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new DomainException("User not found.");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to change password: {errors}");
        }
    }
}
