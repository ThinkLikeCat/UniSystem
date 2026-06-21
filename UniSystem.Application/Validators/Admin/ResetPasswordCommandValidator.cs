using FluentValidation;
using UniSystem.Application.Admin.Commands.ResetPassword;

namespace UniSystem.Application.Validators.Admin;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
