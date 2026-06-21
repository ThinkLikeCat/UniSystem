namespace UniSystem.Domain.Exceptions;

public class InvalidSpecialtyNameException(string name) : DomainException(
    $"Invalid specialty name '{name}'. Name must be non-empty and at most 150 characters.")
{ }

public class InvalidSpecialtyCodeException(string code) : DomainException(
    $"Invalid specialty code '{code}'. Code must be non-empty and at most 20 characters.")
{ }

public class InvalidSpecialtyDurationException(short duration) : DomainException(
    $"Invalid specialty duration {duration}. Allowed range is 3 to 6 years.")
{ }
