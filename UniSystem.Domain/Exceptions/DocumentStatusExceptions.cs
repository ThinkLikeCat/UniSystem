namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentStatusNameException(string name) : DomainException(
    $"Имя статуса документа '{name}' невалидно. Имя должно быть непустым и не более 50 символов.")
{ }
