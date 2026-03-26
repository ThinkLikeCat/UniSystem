using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class Statement: Entity<StatementId>
{
    public Student Student { get; private set; } = new();
    public Secretary Secretary { get; private set; } = new();
    public Dean Dean { get; private set; } = new();
    
    public bool IsSecretaryApprove { get; private set; } // need an enum (or some ValueObject) for this
    public bool IsDeanApprove { get; private set; } // need an enum (or some ValueObject) for this
}