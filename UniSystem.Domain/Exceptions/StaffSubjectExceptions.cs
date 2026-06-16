namespace UniSystem.Domain.Exceptions;

public class InvalidStaffSubjectReferenceException(int subjectId) : DomainException(
    $"Указан невалидный ID предмета: {subjectId}.")
{ }
