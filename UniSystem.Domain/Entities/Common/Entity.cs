namespace UniSystem.Domain.Entities.Common;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}