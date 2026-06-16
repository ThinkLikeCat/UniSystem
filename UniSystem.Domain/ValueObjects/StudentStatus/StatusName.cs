using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.StudentStatus;

public record StatusName
{
    public const int MaxLength = 50;
    
    public string Value { get; private set; }

    public StatusName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidStudentStatusNameException(value);

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new InvalidStudentStatusNameException(value);
        
        Value = cleanedName;
    }
    
    public static implicit operator string(StatusName name) => name.Value;
}