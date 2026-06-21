using FluentValidation;
using UniSystem.Application.Admin.Commands.UpdateUser;
using UniSystem.Domain.Enums;

namespace UniSystem.Application.Validators.Admin;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).MaximumLength(50).When(x => x.FirstName is not null);
        RuleFor(x => x.LastName).MaximumLength(55).When(x => x.LastName is not null);
        RuleFor(x => x.Patronymic).MaximumLength(60).When(x => x.Patronymic is not null);
        RuleFor(x => x.Sex).IsInEnum().When(x => x.Sex is not null);
        RuleFor(x => x.Role).IsInEnum().When(x => x.Role is not null);
    }
}
