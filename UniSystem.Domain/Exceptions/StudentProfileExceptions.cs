namespace UniSystem.Domain.Exceptions;

public class InvalidStudentGroupReferenceException(int academicGroupId) : DomainException(
    $"Invalid academic group ID: {academicGroupId}.")
{ }

public class InvalidStudentStatusReferenceException(int studentStatusId) : DomainException(
    $"Invalid student status ID: {studentStatusId}.")
{ }
