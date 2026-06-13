namespace UniSystem.Domain.Common;

public abstract class Entity<TId> where TId : IEntityId<TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id) => Id = id;
    
    protected Entity() { }
}