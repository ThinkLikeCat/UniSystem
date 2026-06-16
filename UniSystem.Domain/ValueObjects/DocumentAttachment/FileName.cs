using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentAttachment;

public record FileName
{
    public const int MaxFileNameLength = 255;
    
    private static readonly string[] AllowedExtensions =
    [
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".txt",
        ".png",
        ".jpg",
        ".jpeg"
    ];
    
    public string Value { get; private set; }

    public FileName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidAttachmentFileNameException(value);

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxFileNameLength)
            throw new InvalidAttachmentFileNameException(value);
        
        var extension = Path.GetExtension(cleanedName).ToLowerInvariant();
        
        if (!AllowedExtensions.Contains(extension))
            throw new InvalidAttachmentFileNameException(value);
        
        Value = cleanedName;
    }
    
    public static implicit operator string(FileName fileName) => fileName.Value; 
}