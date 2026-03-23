namespace UniSystem.Domain.ValueObjects;

public record struct UniversitySubjectId(Guid Id)
{
    public static UniversitySubjectId NewId() => new(Guid.NewGuid());
    
    public static UniversitySubjectId Empty() => new(Guid.Empty);
}