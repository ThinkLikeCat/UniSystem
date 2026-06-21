using UniSystem.Domain.Enums;

namespace UniSystem.Domain.Exceptions;

public class InvalidUserFirstNameException(string firstName) : DomainException(
    $"Invalid first name '{firstName}'. First name must be non-empty and at most 50 characters.")
{ }

public class InvalidUserLastNameException(string lastName) : DomainException(
    $"Invalid last name '{lastName}'. Last name must be non-empty and at most 55 characters.")
{ }

public class InvalidUserPatronymicException(string patronymic) : DomainException(
    $"Invalid patronymic '{patronymic}'. Patronymic must be non-empty and at most 60 characters.")
{ }

public class InvalidUserSexException(Sex sex) : DomainException(
    $"Invalid sex value: '{sex}'.")
{ }

public class InvalidUserIconException(string path) : DomainException(
    $"Invalid icon path '{path}'.")
{ }

public class UserAlreadyHasStudentProfileException() : DomainException(
    "User already has a student profile.")
{ }

public class UserAlreadyHasStaffProfileException() : DomainException(
    "User already has a staff profile.")
{ }


