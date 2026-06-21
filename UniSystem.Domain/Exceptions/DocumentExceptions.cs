namespace UniSystem.Domain.Exceptions;

public class InvalidDocumentTypeReferenceException(int documentTypeId) : DomainException(
    $"Invalid document type ID: {documentTypeId}.")
{ }

public class InvalidDocumentStatusReferenceException(int statusId) : DomainException(
    $"Invalid document status ID: {statusId}.")
{ }
