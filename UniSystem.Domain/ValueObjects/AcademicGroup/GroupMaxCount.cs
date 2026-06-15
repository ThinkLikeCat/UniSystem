using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.AcademicGroup;

public record GroupMaxCount
{
    private const int MinAllowed = 0;
    private const int MaxAllowed = 35;

    public int Value { get; private set; }

    public GroupMaxCount(int value)
    {
        if (value < MinAllowed || value > MaxAllowed)
            throw new GroupSizeOutOfRangeException(value, MinAllowed, MaxAllowed);

        Value = value;
    }
    
    public bool CanAccommodate(int currentCount) => Value > currentCount;

    public static implicit operator int(GroupMaxCount maxCount) => maxCount.Value;
}