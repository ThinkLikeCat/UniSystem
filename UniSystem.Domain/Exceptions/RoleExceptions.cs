using UniSystem.Domain.Enums;

namespace UniSystem.Domain.Exceptions;

public class InvalidRoleNameException(string name) : DomainException(
    $"Имя роли '{name}' невалидно.")
{ }

public class InvalidRoleSystemNameException(SystemRoleName systemName) : DomainException(
    $"Указано неопределенное системное имя роли: '{systemName}'.")
{ }
