namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentTypeNameException(string name) : DomainException(
    $"Имя типа документа '{name}' невалидно. Имя должно быть непустым и не более 100 символов.")
{ }

public class InvalidTemplateTextException() : DomainException("Текст шаблона не может быть пустым.")
{ }

public class UnknownTemplatePlaceholderException(string placeholder) : DomainException(
    $"Шаблон содержит неизвестный системе тег: '{placeholder}'.")
{ }

public class MissingRequiredTemplatePlaceholderException(string placeholder) : DomainException(
    $"Ошибка шаблона. Отсутствует обязательный тег: {placeholder}.")
{ }
