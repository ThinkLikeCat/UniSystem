namespace UniSystem.Domain.Exceptions;

public class InvalidStudentGroupReferenceException(int academicGroupId) : DomainException(
    $"Указан невалидный ID академической группы: {academicGroupId}.")
{ }

public class InvalidStudentStatusReferenceException(int studentStatusId) : DomainException(
    $"Указан невалидный ID статуса студента: {studentStatusId}.")
{ }
