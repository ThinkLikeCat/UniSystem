using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects;

public record struct UniversityGroupId(Guid Id): IEntityId<UniversityGroupId>
{
    public static UniversityGroupId NewId() => new(Guid.NewGuid());
}