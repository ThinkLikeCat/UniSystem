using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyName
{
    public const int MaxLength = 150;
    public string Value { get; private set; }

    public SpecialtyName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidSpecialtyNameException(value);

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new InvalidSpecialtyNameException(value);

        Value = cleanedName;
    }

    public static implicit operator string(SpecialtyName name) => name.Value;
}