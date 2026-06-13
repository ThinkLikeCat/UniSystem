namespace UniSystem.Domain.Common;

public interface IEntityId<TId> where TId : IEntityId<TId>
{
    Guid Value { get; }
}
