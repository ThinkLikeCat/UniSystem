using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record IconPath
{
    public const int MaxLength = 255;
    public string Value { get; private set; }

    public IconPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserIconException(value);

        var cleanedPath = value.Trim();

        if (cleanedPath.Length > MaxLength)
            throw new InvalidUserIconException(value);
        
        if (cleanedPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new InvalidUserIconException(value);

        Value = cleanedPath.Replace('\\', '/');
    }
    
    public static implicit operator string(IconPath path) => path.Value;
}