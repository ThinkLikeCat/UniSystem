using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects.Id;

public record struct SpecialityId(Guid Id): IEntityId<SpecialityId>
{
    public static SpecialityId NewId() => new(Guid.NewGuid());
}