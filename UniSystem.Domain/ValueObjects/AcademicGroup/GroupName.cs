using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.AcademicGroup;

public record GroupName
{
    public const int MaxLength = 20;
    public string Value { get; init; } = string.Empty;

    public GroupName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidAcademicGroupNameException("Имя группы не может быть пустым.");

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new InvalidAcademicGroupNameException(cleanedName);

        Value = cleanedName;
    }

    public static implicit operator string(GroupName name) => name.Value;
}