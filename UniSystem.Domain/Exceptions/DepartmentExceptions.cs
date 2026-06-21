namespace UniSystem.Domain.Exceptions;

public class InvalidDepartmentNameException(string name) : DomainException(
    $"Invalid department name '{name}'. Name must be non-empty and at most 150 characters.")
{ }
