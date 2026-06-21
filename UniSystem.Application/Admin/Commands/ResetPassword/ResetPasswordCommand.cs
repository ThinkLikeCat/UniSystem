using MediatR;
using Microsoft.AspNetCore.Identity;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Admin.Commands.ResetPassword;

public record ResetPasswordCommand(Guid UserId, string NewPassword) : IRequest;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new DomainException("User not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new DomainException($"Failed to reset password: {errors}");
        }
    }
}
