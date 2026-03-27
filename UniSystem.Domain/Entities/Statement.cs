using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Interfaces;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Statement: Entity<StatementId>
{
    public Student Student { get; private set; } = new();
    public Secretary Secretary { get; private set; } = new();
    public Dean Dean { get; private set; } = new();

    public IStatementModels StatementModel { get; private set; }

    public SecretaryReviewStatus IsSecretaryApprove { get; private set; } = SecretaryReviewStatus.Rejected;
    public DeanReviewStatus IsDeanApprove { get; private set; } = DeanReviewStatus.Rejected;
}