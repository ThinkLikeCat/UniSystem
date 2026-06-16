using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Role;

public record RoleNameDative
{
    public const int MaxLength = 55;
    public string Value { get; private set; }

    public RoleNameDative(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidRoleNameException(value);

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new InvalidRoleNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(RoleNameDative name) => name.Value;
}