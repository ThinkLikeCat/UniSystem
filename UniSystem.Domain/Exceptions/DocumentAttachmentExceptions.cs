namespace UniSystem.Domain.Exceptions;

public class InvalidAttachmentFileNameException(string fileName) : DomainException(
    $"Имя файла '{fileName}' невалидно.")
{ }

public class InvalidAttachmentFilePathException(string path) : DomainException(
    $"Путь к файлу '{path}' невалиден.")
{ }

public class InvalidAttachmentFileSizeException(long size) : DomainException(
    $"Размер файла {size} невалиден. Размер не может быть отрицательным.")
{ }
