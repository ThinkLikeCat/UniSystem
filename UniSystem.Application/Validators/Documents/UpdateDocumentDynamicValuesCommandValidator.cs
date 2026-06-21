using FluentValidation;
using UniSystem.Application.Documents.Commands;

namespace UniSystem.Application.Validators.Documents;

public class UpdateDocumentDynamicValuesCommandValidator : AbstractValidator<UpdateDocumentDynamicValuesCommand>
{
    public UpdateDocumentDynamicValuesCommandValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.DynamicValues).NotEmpty();
    }
}
