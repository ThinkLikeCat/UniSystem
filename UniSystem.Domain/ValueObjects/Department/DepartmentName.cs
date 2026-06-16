using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Department;

public record DepartmentName
{
    public const int MaxLength = 150;
    public string Value { get; private set; }

    public DepartmentName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidDepartmentNameException(value);

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new InvalidDepartmentNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(DepartmentName name) => name.Value;
}