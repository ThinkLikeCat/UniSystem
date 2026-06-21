using FluentValidation;
using UniSystem.Application.Documents.Commands.DeanReview;

namespace UniSystem.Application.Validators.Documents;

public class DeanReviewCommandValidator : AbstractValidator<DeanReviewCommand>
{
    public DeanReviewCommandValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.Decision).IsInEnum();
        When(x => x.ResolutionComment is not null, () =>
            RuleFor(x => x.ResolutionComment!).MaximumLength(1000));
    }
}
