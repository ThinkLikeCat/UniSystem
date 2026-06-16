using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record LastName
{
    public const int MaxLength = 55;
    public string Value { get; private set; }

    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserLastNameException(value);

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new InvalidUserLastNameException(value);

        Value = cleaned;
    }
    
    public static implicit operator string(LastName lastName) => lastName.Value;
}