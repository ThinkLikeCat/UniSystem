using System.Text.RegularExpressions;

namespace UniSystem.Domain.ValueObjects.User;

public record Email
{
    public const int MaxLength = 255;
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    
    public string Value { get; private set; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email не может быть пустым.", nameof(value));

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new ArgumentException($"Email не может превышать {MaxLength} символов.", nameof(value));

        if (!EmailRegex.IsMatch(cleaned))
            throw new ArgumentException("Некорректный формат Email.", nameof(value));

        Value = cleaned;
    }
    
    public static implicit operator string(Email email) => email.Value;
}