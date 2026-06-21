namespace UniSystem.Domain.Exceptions;

public class InvalidSubjectNameException(string name) : DomainException(
    $"Invalid subject name '{name}'. Name must be non-empty and at most 150 characters.")
{ }
