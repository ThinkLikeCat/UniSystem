using UniSystem.Domain.Enums;

namespace UniSystem.Domain.Exceptions;

public class InvalidRoleNameException(string name) : DomainException(
    $"Invalid role name '{name}'.")
{ }

public class InvalidRoleSystemNameException(SystemRoleName systemName) : DomainException(
    $"Invalid system role name: '{systemName}'.")
{ }
