using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record FirstName
{
    public const int MaxLength = 50;
    public string Value { get; private set; }

    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserFirstNameException(value);

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new InvalidUserFirstNameException(value);

        Value = cleaned;
    }
    
    public static implicit operator string(FirstName firstName) => firstName.Value;
}