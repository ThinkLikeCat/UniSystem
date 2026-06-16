using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentType;

public record DocumentTypeName
{
    public const int MaxLength = 100;
    public string Value { get; private set; }

    public DocumentTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidDocumentTypeNameException(value);

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new InvalidDocumentTypeNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(DocumentTypeName name) => name.Value;
}