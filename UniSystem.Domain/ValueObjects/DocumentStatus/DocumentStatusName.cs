namespace UniSystem.Domain.ValueObjects.DocumentStatus;

public record DocumentStatusName
{
    public const int MaxLength = 50;
    public string Value { get; init; } = string.Empty;

    public DocumentStatusName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(value));

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new ArgumentException("Имя не может быть длиннее 50 символов.", nameof(value));

        Value = cleanedName;
    }

    public static implicit operator string(DocumentStatusName name) => name.Value;
}