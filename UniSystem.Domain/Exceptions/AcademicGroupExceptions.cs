using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Exceptions;

public class InvalidAcademicGroupNameException(string groupName) : DomainException(
    $"Invalid academic group name: '{groupName}'. Name must be non-empty and at most 20 characters.") 
{ }

public class GroupSizeOutOfRangeException(int count, int min, int max) : DomainException (
    $"Group size {count} is out of range. Allowed range is [{min}..{max}].") 
{ }

public class InvalidSpecialtyReferenceException(int specialtyId) : DomainException(
    $"Указан невалидный Id специальности: {specialtyId}.")
{ }

public class StudentAlreadyInGroupException(UserId studentId, string groupName) : DomainException(
    $"Студент с Id {studentId.Value} уже состоит в группе '{groupName}'.")
{ }

public class GroupIsFullException(string groupName) : DomainException(
    $"Группа '{groupName}' переполнена.")
{ }

public class StudentNotInGroupException(UserId studentId, string groupName) : DomainException(
    $"Студент с ID {studentId.Value} не принадлежит группе '{groupName}'.")
{ }

public class CourseOutOfRangeException(short course) : DomainException(
    $"Курс {course} невалиден. Допустимый диапазон: от 1 до 6.")
{ }

