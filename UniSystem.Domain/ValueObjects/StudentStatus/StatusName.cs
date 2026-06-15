namespace UniSystem.Domain.ValueObjects.StudentStatus;

public record StatusName
{
    public const int MaxLength = 50;
    
    public string Value { get; private set; }

    public StatusName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(value));

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new ArgumentException($"Имя не может превышать {MaxLength} символов.", nameof(value));
        
        Value = cleanedName;
    }
    
    public static implicit operator string(StatusName name) => name.Value;
}