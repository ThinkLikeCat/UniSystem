using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects.Id;

public record struct DeanId(Guid Id): IEntityId<DeanId>
{
    public static DeanId NewId() => new(Guid.NewGuid());
}