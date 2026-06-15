namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyCode
{
    public const int MaxLength = 20;
    public string Value { get; private set; }

    public SpecialtyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Код не может быть пустым.", nameof(value));

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new ArgumentException("Код не может превышать 20 символов.", nameof(value));

        Value = cleanedName;
    }

    public static implicit operator string(SpecialtyCode code) => code.Value;
}