namespace UniSystem.Domain.ValueObjects.Specialty;

public record SpecialtyDuration
{
    public const int MinDuration = 3;
    public const int MaxDuration = 6;

    public short Value { get; init; }

    public SpecialtyDuration(short value)
    {
        if (value < MinDuration || value > MaxDuration)
            throw new ArgumentException($"Продолжительность обучения должна быть в диапазоне от {MinDuration} до {MaxDuration}", nameof(value));

        Value = value;
    }

    public static implicit operator short(SpecialtyDuration duration) => duration.Value;
}