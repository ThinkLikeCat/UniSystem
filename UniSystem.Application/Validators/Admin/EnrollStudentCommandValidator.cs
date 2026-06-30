using FluentValidation;
using UniSystem.Application.Admin.Commands.EnrollStudent;

namespace UniSystem.Application.Validators.Admin;

public class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
{
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.GroupId).GreaterThan(0);
    }
}
