namespace UniSystem.Domain.Exceptions;

public class InvalidStaffDepartmentReferenceException(int departmentId) : DomainException(
    $"Invalid department ID: {departmentId}.")
{ }

public class InvalidStaffAcademicGroupReferenceException(int? academicGroupId) : DomainException(
    $"Invalid academic group ID: {academicGroupId}.")
{ }
