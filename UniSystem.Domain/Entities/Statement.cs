using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class Statement: Entity<StatementId>
{ 
    public string Type { get; private set; }
}