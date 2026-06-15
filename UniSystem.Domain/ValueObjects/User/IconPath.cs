using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record IconPath
{
    public const int MaxLength = 255;
    public string Value { get; private set; }

    public IconPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Путь к иконке не может быть пустым.");

        var cleanedPath = value.Trim();

        if (cleanedPath.Length > MaxLength)
            throw new DomainException($"Путь к иконке не может превышать {MaxLength} символов.");
        
        if (cleanedPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new DomainException("Путь к файлу содержит недопустимые символы.");

        Value = cleanedPath.Replace('\\', '/');
    }
    
    public static implicit operator string(IconPath path) => path.Value;
}