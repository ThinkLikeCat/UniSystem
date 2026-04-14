using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.Common;

public abstract class Entity<TId> where TId : struct, IEntityId<TId>
{
    public TId Id { get; init; }

    public DateTime CreatedAt { get; init; }

    protected Entity()
    {
        Id = TId.NewId();
        CreatedAt = DateTime.UtcNow;
    }

    protected Entity(TId id, DateTime createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }

    public override int GetHashCode() => Id.GetHashCode();
}