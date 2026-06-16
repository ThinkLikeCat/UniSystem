namespace UniSystem.Domain.Exceptions;

public class InvalidSpecialtyNameException(string name) : DomainException(
    $"Наименование специальности '{name}' невалидно. Наименование должно быть непустым и не более 150 символов.")
{ }

public class InvalidSpecialtyCodeException(string code) : DomainException(
    $"Код специальности '{code}' невалиден. Код должен быть непустым и не более 20 символов.")
{ }

public class InvalidSpecialtyDurationException(short duration) : DomainException(
    $"Продолжительность обучения {duration} невалидна. Допустимый диапазон: от 3 до 6 лет.")
{ }
