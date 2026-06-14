namespace UniSystem.Domain.Common;

public abstract class Entity<TId> where TId : IEntityId<TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id) => Id = id;
    
    protected Entity() { }

    public override int GetHashCode() => Id is null ? base.GetHashCode() : Id.GetHashCode();
    
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        
        if (Id is null || other.Id is null) return false;

        return Id.Equals(other.Id);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals(obj as Entity<TId>);
    }
    
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }
    
    public static bool operator != (Entity<TId>? left, Entity<TId>? right) =>
        !(left == right);
}