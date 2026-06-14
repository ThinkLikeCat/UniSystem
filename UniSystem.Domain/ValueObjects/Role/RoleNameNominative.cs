namespace UniSystem.Domain.ValueObjects.Role;

public record RoleNameNominative
{
    public const int MaxLength = 50;
    public string Value { get; init; } = string.Empty;

    public RoleNameNominative(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя в именительном падеже не может быть пустым.", nameof(value));

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new ArgumentException("Имя в именительном падеже не может превышать 50 символов.", nameof(value));

        Value = cleanedName;
    }

    public static implicit operator string(RoleNameNominative name) => name.Value;
}