using FluentValidation;
using UniSystem.Application.Documents.Commands;

namespace UniSystem.Application.Validators.Documents;

public class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
{
    public UploadAttachmentCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.FileSize).InclusiveBetween(1, 10 * 1024 * 1024);
    }
}
