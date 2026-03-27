using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects.Id;

public record struct TeacherId(Guid Id): IEntityId<TeacherId>
{
    public static TeacherId NewId() => new(Guid.NewGuid());
}