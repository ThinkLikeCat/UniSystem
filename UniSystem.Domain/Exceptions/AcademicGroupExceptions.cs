namespace UniSystem.Domain.Exceptions;

public class InvalidAcademicGroupNameException(string groupName) : DomainException(
    $"Invalid academic group name: '{groupName}'. Name must be non-empty and at most 20 characters.") 
{ }

public class GroupSizeOutOfRangeException(int count, int min, int max) : DomainException (
    $"Group size {count} is out of range. Allowed range is [{min}..{max}].") 
{ }

public class InvalidSpecialtyReferenceException(int specialtyId) : DomainException(
    $"Invalid specialty ID: {specialtyId}.")
{ }

public class StudentAlreadyInGroupException(Guid studentId, string groupName) : DomainException(
    $"Student with ID {studentId} is already in group '{groupName}'.")
{ }

public class GroupIsFullException(string groupName) : DomainException(
    $"Group '{groupName}' is full.")
{ }

public class StudentNotInGroupException(Guid studentId, string groupName) : DomainException(
    $"Student with ID {studentId} does not belong to group '{groupName}'.")
{ }

public class CourseOutOfRangeException(short course) : DomainException(
    $"Course {course} is out of range. Allowed range is 1 to 6.")
{ }

