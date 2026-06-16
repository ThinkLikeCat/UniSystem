using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Role;

public record RoleNameNominative
{
    public const int MaxLength = 50;
    public string Value { get; private set; }

    public RoleNameNominative(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidRoleNameException(value);

        var cleanedName = value.Trim();
        
        if (cleanedName.Length > MaxLength)
            throw new InvalidRoleNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(RoleNameNominative name) => name.Value;
}