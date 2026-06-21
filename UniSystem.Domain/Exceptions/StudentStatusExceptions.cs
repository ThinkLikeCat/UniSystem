namespace UniSystem.Domain.Exceptions;

public class InvalidStudentStatusNameException(string name) : DomainException(
    $"Invalid student status name '{name}'. Name must be non-empty and at most 50 characters.")
{ }
