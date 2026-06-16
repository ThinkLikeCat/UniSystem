using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentAttachment;

public record FilePath
{
    public const int MaxLength = 500;

    public string Value { get; private set; }

    public FilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidAttachmentFilePathException(value);

        var cleanedPath = value.Trim();

        if (cleanedPath.Length > MaxLength)
            throw new InvalidAttachmentFilePathException(value);
        
        if (cleanedPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new InvalidAttachmentFilePathException(value);

        Value = cleanedPath.Replace('\\', '/');
    }

    public static implicit operator string(FilePath path) => path.Value;
}