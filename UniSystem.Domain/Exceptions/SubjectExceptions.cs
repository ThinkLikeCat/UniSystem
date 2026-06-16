namespace UniSystem.Domain.Exceptions;

public class InvalidSubjectNameException(string name) : DomainException(
    $"Имя предмета '{name}' невалидно. Имя должно быть непустым и не более 150 символов.")
{ }
