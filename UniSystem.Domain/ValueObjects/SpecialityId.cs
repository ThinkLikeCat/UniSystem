namespace UniSystem.Domain.ValueObjects;

public record struct SpecialityId(Guid Id)
{
    public static SpecialityId NewId() => new(Guid.NewGuid());

    public static SpecialityId Empty() => new(Guid.Empty);
}