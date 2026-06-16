namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentTypeReferenceException(int documentTypeId) : DomainException(
    $"Указан невалидный ID типа документа: {documentTypeId}.")
{ }

public class InvalidDocumentStatusReferenceException(int statusId) : DomainException(
    $"Указан невалидный ID статуса документа: {statusId}.")
{ }
