using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyDuration
{
    public const int MinDuration = 3;
    public const int MaxDuration = 6;

    public short Value { get; private set; }

    public SpecialtyDuration(short value)
    {
        if (value < MinDuration || value > MaxDuration)
            throw new InvalidSpecialtyDurationException(value);

        Value = value;
    }

    public static implicit operator short(SpecialtyDuration duration) => duration.Value;
}