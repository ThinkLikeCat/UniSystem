namespace UniSystem.Domain.Interfaces;

public interface IEntityId<out TId> where TId: IEntityId<TId>
{
    public static abstract TId NewId();
}