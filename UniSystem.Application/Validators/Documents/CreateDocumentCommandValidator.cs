using System.Text.Json;
using FluentValidation;
using UniSystem.Application.Documents.Commands;

namespace UniSystem.Application.Validators.Documents;

public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
{
    public CreateDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentTypeId).GreaterThan(0);

        When(x => x.DynamicValues is not null, () =>
        {
            RuleFor(x => x.DynamicValues!)
                .Must(BeValidJson)
                .WithMessage("DynamicValues must be a valid JSON string for the jsonb column.");
        });
    }

    private static bool BeValidJson(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;
        try
        {
            JsonDocument.Parse(value);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
