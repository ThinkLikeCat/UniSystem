using FluentValidation;
using UniSystem.Application.Admin.Commands.AssignCurator;

namespace UniSystem.Application.Validators.Admin;

public class AssignCuratorCommandValidator : AbstractValidator<AssignCuratorCommand>
{
    public AssignCuratorCommandValidator()
    {
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.GroupId).GreaterThan(0);
    }
}
