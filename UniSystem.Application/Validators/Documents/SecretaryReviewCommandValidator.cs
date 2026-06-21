using FluentValidation;
using UniSystem.Application.Documents.Commands.SecretaryReview;

namespace UniSystem.Application.Validators.Documents;

public class SecretaryReviewCommandValidator : AbstractValidator<SecretaryReviewCommand>
{
    public SecretaryReviewCommandValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.Decision).IsInEnum();
        When(x => x.ResolutionComment is not null, () =>
            RuleFor(x => x.ResolutionComment!).MaximumLength(1000));
    }
}
