using FluentValidation;
using UniSystem.Application.Users.Commands.UpdateStudentProfile;

namespace UniSystem.Application.Validators.Users;

public class UpdateStudentProfileCommandValidator : AbstractValidator<UpdateStudentProfileCommand>
{
    public UpdateStudentProfileCommandValidator()
    {
        When(x => x.StudentTicket is not null, () =>
            RuleFor(x => x.StudentTicket!).MaximumLength(50));
    }
}
