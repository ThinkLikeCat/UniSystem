using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.Entities.Common;

public abstract class Entity<TId> where TId: struct, IEntityId<TId>
{
    public TId Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    protected Entity()
    {
        Id = TId.NewId();
        CreatedAt = DateTime.UtcNow;
    }
    
    protected Entity(TId id) { Id = id; }
}