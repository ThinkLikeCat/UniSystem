namespace UniSystem.Domain.ValueObjects;

public record struct TeacherId(Guid Id)
{
    public static TeacherId NewId() => new TeacherId(Guid.NewGuid());
    
    public static TeacherId Empty => new(Guid.Empty);
}