using UniSystem.Domain.Interfaces;

namespace UniSystem.Domain.ValueObjects.Id;

public record struct SecretaryId(Guid Id) : IEntityId<SecretaryId>
{
    public static SecretaryId NewId() => new(Guid.NewGuid());
}