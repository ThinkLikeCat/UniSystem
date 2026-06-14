namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyName
{
    public const int MaxLength = 150;
    public string Value { get; init; } = string.Empty;

    public SpecialtyName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Наименование не может быть пустым или состоять только из пробелов.", nameof(value));

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new ArgumentException("Наименование не может превышать 150 символов.", nameof(value));

        Value = cleanedName;
    }

    public static implicit operator string(SpecialtyName name) => name.Value;
}