namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentTypeNameException(string name) : DomainException(
    $"Invalid document type name '{name}'. Name must be non-empty and at most 100 characters.")
{ }

public class InvalidTemplateTextException() : DomainException("Template text cannot be empty.")
{ }

public class UnknownTemplatePlaceholderException(string placeholder) : DomainException(
    $"Template contains an unknown system placeholder: '{placeholder}'.")
{ }

public class MissingRequiredTemplatePlaceholderException(string placeholder) : DomainException(
    $"Template error. Missing required placeholder: {placeholder}.")
{ }
