using System.Text.RegularExpressions;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.User;

public record Email
{
    public const int MaxLength = 255;
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    
    public string Value { get; private set; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUserEmailException(value);

        var cleaned = value.Trim();

        if (cleaned.Length > MaxLength)
            throw new InvalidUserEmailException(value);

        if (!EmailRegex.IsMatch(cleaned))
            throw new InvalidUserEmailException(value);

        Value = cleaned;
    }
    
    public static implicit operator string(Email email) => email.Value;
}