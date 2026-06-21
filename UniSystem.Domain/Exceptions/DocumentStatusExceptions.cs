namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentStatusNameException(string name) : DomainException(
    $"Invalid document status name '{name}'. Name must be non-empty and at most 50 characters.")
{ }
