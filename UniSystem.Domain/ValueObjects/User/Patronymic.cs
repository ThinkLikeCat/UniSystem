using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record Patronymic
{
    public const int MaxLength = 60;
    public string Value { get; private set; }

    public Patronymic(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserPatronymicException(value);

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new InvalidUserPatronymicException(value);

        Value = cleaned;
    }
    
    public override string ToString() => Value;
    public static implicit operator string(Patronymic patronymic) => patronymic.Value;
}