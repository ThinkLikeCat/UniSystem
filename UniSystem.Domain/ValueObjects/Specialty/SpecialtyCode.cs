using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyCode
{
    public const int MaxLength = 20;
    public string Value { get; private set; }

    public SpecialtyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidSpecialtyCodeException(value);

        var cleanedName = value.Trim();

        if (cleanedName.Length > MaxLength)
            throw new InvalidSpecialtyCodeException(value);

        Value = cleanedName;
    }

    public static implicit operator string(SpecialtyCode code) => code.Value;
}