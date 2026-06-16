using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentStatus;

public record DocumentStatusName
{
    public const int MaxLength = 50;
    public string Value { get; private set; }

    public DocumentStatusName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidDocumentStatusNameException(value);

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new InvalidDocumentStatusNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(DocumentStatusName name) => name.Value;
}