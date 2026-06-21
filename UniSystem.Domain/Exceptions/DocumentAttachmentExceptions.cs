namespace UniSystem.Domain.Exceptions;

public class InvalidAttachmentFileNameException(string fileName) : DomainException(
    $"Invalid file name '{fileName}'.")
{ }

public class InvalidAttachmentFilePathException(string path) : DomainException(
    $"Invalid file path '{path}'.")
{ }

public class InvalidAttachmentFileSizeException(long size) : DomainException(
    $"Invalid file size {size}. Size cannot be negative.")
{ }
