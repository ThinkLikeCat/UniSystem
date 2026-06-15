namespace UniSystem.Domain.ValueObjects.User;

public record Patronymic
{
    public const int MaxLength = 60;
    public string Value { get; private set; }

    public Patronymic(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Отчество не может состоять только из пробелов.", nameof(value));

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new ArgumentException($"Отчество не может превышать {MaxLength} символов.", nameof(value));

        Value = cleaned;
    }
    
    public static implicit operator string(Patronymic patronymic) => patronymic.Value;
}