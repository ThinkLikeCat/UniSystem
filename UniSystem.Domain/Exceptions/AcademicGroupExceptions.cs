namespace UniSystem.Domain.Exceptions;

public class InvalidAcademicGroupNameException : DomainException
{
    public string? GroupName { get; }

    public InvalidAcademicGroupNameException(string groupName)
        : base($"Invalid academic group name: '{groupName}'. Name must be non-empty and at most 20 characters.")
    {
        GroupName = groupName;
    }
}

public class GroupSizeOutOfRangeException : DomainException
{
    public int Count { get; }
    public int Min { get; }
    public int Max { get; }

    public GroupSizeOutOfRangeException(int count, int min, int max)
        : base($"Group size {count} is out of range. Allowed range is [{min}..{max}].")
    {
        Count = count;
        Min = min;
        Max = max;
    }
}

public class CurrentCountOutOfRangeException : DomainException
{
    public int CurrentCount { get; }
    public int MaxAllowed { get; }

    public CurrentCountOutOfRangeException(int currentCount, int maxAllowed)
        : base($"Current count {currentCount} is invalid. It must be between 0 and {maxAllowed}.")
    {
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }
}

