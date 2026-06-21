using FluentValidation;
using UniSystem.Application.Users.Commands.UpdateStaffProfile;

namespace UniSystem.Application.Validators.Users;

public class UpdateStaffProfileCommandValidator : AbstractValidator<UpdateStaffProfileCommand>
{
    // all fields are optional; no specific rules needed
}
