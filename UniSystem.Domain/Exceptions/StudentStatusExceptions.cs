namespace UniSystem.Domain.Exceptions;

public class InvalidStudentStatusNameException(string name) : DomainException(
    $"Имя статуса студента '{name}' невалидно. Имя должно быть непустым и не более 50 символов.")
{ }
