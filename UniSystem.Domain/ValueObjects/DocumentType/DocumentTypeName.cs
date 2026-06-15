namespace UniSystem.Domain.ValueObjects.DocumentType;

public record DocumentTypeName
{
    public const int MaxLength = 100;
    public string Value { get; private set; }

    public DocumentTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(value));

        var cleanedPath = value.Trim();
        
        if (cleanedPath.Length > MaxLength)
            throw new ArgumentException("Имя не может превышать 100 символов.", nameof(value));

        Value = cleanedPath;
    }

    public static implicit operator string(DocumentTypeName name) => name.Value;
}