using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentAttachment;

public record FilePath
{
    public const int MaxLength = 500;

    public string Value { get; init; } = string.Empty;

    public FilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Путь к файлу не может быть пустым.");

        var cleanedPath = value.Trim();

        if (cleanedPath.Length > MaxLength)
            throw new DomainException($"Путь к файлу не может превышать {MaxLength} символов.");
        
        if (cleanedPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new DomainException("Путь к файлу содержит недопустимые символы.");

        Value = cleanedPath.Replace('\\', '/');
    }

    public static implicit operator string(FilePath path) => path.Value;
}