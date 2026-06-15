namespace UniSystem.Domain.ValueObjects.Subject;

public record SubjectName
{
    public const int MaxLength = 150;
    
    public string Value { get; private set; }

    public SubjectName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(value));

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new ArgumentException($"Имя не может превышать {MaxLength} символов.", nameof(value));
        
        Value = cleanedName;
    }
    
    public static implicit operator string(SubjectName name) => name.Value;
}