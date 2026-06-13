namespace UniSystem.Domain.ValueObjects;

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
    
    public string Value { get; init; } = string.Empty;

    public FileName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Некорректное имя файла.");

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxFileNameLength)
            throw new ArgumentException($"Имя файла не может превышать {MaxFileNameLength} символов.");
        
        var extension = Path.GetExtension(value).ToLowerInvariant();
        
        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException($"Формат {extension} не поддерживается.");
        
        Value = cleanedName;
    }
    
    public static implicit operator string(FileName fileName) => fileName.Value; 
}