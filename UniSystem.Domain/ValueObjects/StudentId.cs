namespace UniSystem.Domain.ValueObjects;

public record struct StudentId(Guid Id)
{
    public static StudentId NewId() => new(Guid.NewGuid());

    public static StudentId Empty() => new(Guid.Empty);
}