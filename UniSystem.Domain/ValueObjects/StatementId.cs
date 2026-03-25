using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects;

public record struct StatementId(Guid Id): IEntityId<StatementId>
{
    public static StatementId NewId() => new(Guid.NewGuid());
}