namespace UniSystem.Domain.Exceptions;

public class InvalidStaffSubjectReferenceException(int subjectId) : DomainException(
    $"Invalid subject ID: {subjectId}.")
{ }
