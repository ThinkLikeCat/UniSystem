using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record PasswordHash
{
    public string Value { get; private set; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserPasswordHashException();

        Value = value;
    }
    
    public static implicit operator string(PasswordHash hash) => hash.Value;
}