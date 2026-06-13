using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects;

public record FilePath
{
    public const int MaxLength = 500;

    public string Value { get; init; } = string.Empty;

    public FilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Путь к файлу не может быть пустым.");

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new DomainException($"Путь к файлу не может превышать {MaxLength} символов.");

        Value = cleaned;
    }

    public static implicit operator string(FilePath path) => path.Value;
}