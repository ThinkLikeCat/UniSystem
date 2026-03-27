using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects.Id;

public record struct StudentId(Guid Id): IEntityId<StudentId>
{
    public static StudentId NewId() => new(Guid.NewGuid());
}