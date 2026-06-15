using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentAttachment;

public record FileSize
{
    public long Value { get; private set; }

    public FileSize(long value)
    {
        if (value < 0)
            throw new DomainException("Размер файла не может быть отрицательным.");

        Value = value;
    }

    public static implicit operator long(FileSize fileSize) => fileSize.Value;
}