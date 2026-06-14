namespace UniSystem.Domain.Exceptions;

public class InvalidAcademicGroupNameException(string groupName) : DomainException(
    $"Invalid academic group name: '{groupName}'. Name must be non-empty and at most 20 characters.") 
{ }

public class GroupSizeOutOfRangeException(int count, int min, int max) : DomainException (
    $"Group size {count} is out of range. Allowed range is [{min}..{max}].") 
{ }

