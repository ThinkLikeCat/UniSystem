namespace UniSystem.Domain.ValueObjects.Department;

public record DepartmentName
{
    public const int MaxLength = 150;
    public string Value { get; private set; }

    public DepartmentName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя кафедры не может быть пустым.");

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new ArgumentException($"Имя кафедры не может превышать {MaxLength} символов.");

        Value = cleanedName;
    }

    public static implicit operator string(DepartmentName name) => name.Value;
}