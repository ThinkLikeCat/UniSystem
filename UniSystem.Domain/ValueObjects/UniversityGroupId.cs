namespace UniSystem.Domain.ValueObjects;

public record struct UniversityGroupId(Guid Id)
{
    public static UniversityGroupId NewId() => new(Guid.NewGuid());

    public static UniversityGroupId Empty() => new(Guid.Empty);
}