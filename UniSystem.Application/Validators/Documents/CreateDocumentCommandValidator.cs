using FluentValidation;
using UniSystem.Application.Documents.Commands;

namespace UniSystem.Application.Validators.Documents;

public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
{
    public CreateDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentTypeId).GreaterThan(0);
    }
}
