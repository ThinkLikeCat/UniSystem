using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.AcademicGroup;

public record GroupName
{
    public const int MaxLength = 20;
    public string Value { get; private set; }

    public GroupName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidAcademicGroupNameException("Group name cannot be empty.");

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new InvalidAcademicGroupNameException(cleanedName);

        Value = cleanedName;
    }

    public static implicit operator string(GroupName name) => name.Value;
}