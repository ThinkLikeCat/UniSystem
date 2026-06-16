namespace UniSystem.Domain.Exceptions;

public class InvalidDepartmentNameException(string name) : DomainException(
    $"Имя кафедры '{name}' невалидно. Имя должно быть непустым и не более 150 символов.")
{ }
