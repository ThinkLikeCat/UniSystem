using UniSystem.Domain.Enums;

namespace UniSystem.Domain.Exceptions;

public class InvalidUserEmailException(string email) : DomainException(
    $"Email '{email}' имеет неверный формат.")
{ }

public class InvalidUserPasswordHashException() : DomainException(
    "Хэш пароля не может быть пустым.")
{ }

public class InvalidUserFirstNameException(string firstName) : DomainException(
    $"Имя '{firstName}' невалидно. Имя должно быть непустым и не более 50 символов.")
{ }

public class InvalidUserLastNameException(string lastName) : DomainException(
    $"Фамилия '{lastName}' невалидна. Фамилия должна быть непустой и не более 55 символов.")
{ }

public class InvalidUserPatronymicException(string patronymic) : DomainException(
    $"Отчество '{patronymic}' невалидно. Отчество должно быть непустым и не более 60 символов.")
{ }

public class InvalidUserSexException(Sex sex) : DomainException(
    $"Указано неопределенное значение пола: '{sex}'.")
{ }

public class InvalidUserIconException(string path) : DomainException(
    $"Путь к иконке '{path}' невалиден.")
{ }

public class InvalidUserRoleIdException(int roleId) : DomainException(
    $"Указан невалидный ID роли: {roleId}.")
{ }
