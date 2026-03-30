using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Interfaces;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Statement : Entity<StatementId>
{
    public Student Student { get; private set; } = new();
    public Secretary Secretary { get; private set; } = new();
    public Dean Dean { get; private set; } = new();

    public IStatementModels StatementModel { get; private set; }
    public SecretaryReviewStatus SecretaryReview { get; private set; } = SecretaryReviewStatus.Unwatched;
    public bool IsSecretarySign { get; private set; } = false;
    public DeanReviewStatus DeanReview { get; private set; } = DeanReviewStatus.Unwatched;
    public bool IsDeanSign { get; private set; } = false;

    public void Reject(Secretary secretary)
    {
        if (Secretary.Id == secretary.Id)
            SecretaryReview = SecretaryReviewStatus.Rejected;
    }
    
    public void Approve(Secretary secretary)
    {
        if (Secretary.Id == secretary.Id)
            SecretaryReview = SecretaryReviewStatus.Approved;
    }

    public void SignUp(Secretary secretary)
    {
        if (Secretary.Id != secretary.Id)
            throw new ArgumentException();
        
        if(!(SecretaryReview is SecretaryReviewStatus.Rejected or SecretaryReviewStatus.Approved))
            throw new ArgumentException();
            
        IsSecretarySign = true;
    }

    public void SignUp(Dean dean)
    {
        if (Dean.Id != dean.Id) 
            throw new ArgumentException();
        
        if (!IsSecretarySign)
            throw new ArgumentException();

        if (DeanReview is DeanReviewStatus.Reviewed)
        {
            DeanReview = SecretaryReview switch
            {
                SecretaryReviewStatus.Approved => DeanReviewStatus.Approved,
                SecretaryReviewStatus.Rejected => DeanReviewStatus.Rejected,
                SecretaryReviewStatus.Reviewed => DeanReviewStatus.Reviewed,
                SecretaryReviewStatus.Unwatched => DeanReviewStatus.Unwatched,
                _ => DeanReviewStatus.Rejected
            };
        }
        
        IsDeanSign = true;
    }
}