using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects;

public record struct UniversitySubjectId(Guid Id): IEntityId<UniversitySubjectId>
{
    public static UniversitySubjectId NewId() => new(Guid.NewGuid());
}