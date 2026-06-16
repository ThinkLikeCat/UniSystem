namespace UniSystem.Domain.Exceptions;

public class InvalidStaffDepartmentReferenceException(int departmentId) : DomainException(
    $"Указан невалидный ID кафедры: {departmentId}.")
{ }

public class InvalidStaffAcademicGroupReferenceException(int? academicGroupId) : DomainException(
    $"Указан невалидный ID академической группы: {academicGroupId}.")
{ }
