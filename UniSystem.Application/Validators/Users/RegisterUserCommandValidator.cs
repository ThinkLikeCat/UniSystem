using FluentValidation;
using UniSystem.Application.Users.Commands.RegisterUser;
using UniSystem.Domain.Enums;

namespace UniSystem.Application.Validators.Users;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(55);
        RuleFor(x => x.Patronymic).MaximumLength(60);
        RuleFor(x => x.Sex).IsInEnum();
        RuleFor(x => x.Role).IsInEnum();

        When(x => x.Role == SystemRoleName.StudentProfile, () =>
        {
            RuleFor(x => x.StudentTicket).NotEmpty().MaximumLength(50);
            RuleFor(x => x.AcademicGroupId).NotNull();
            RuleFor(x => x.StudentStatusId).NotNull();
        });

        When(x => x.Role is SystemRoleName.StaffProfile or SystemRoleName.Dean
            or SystemRoleName.Secretary or SystemRoleName.Curator, () =>
        {
            RuleFor(x => x.DepartmentId).NotNull();
        });
    }
}
