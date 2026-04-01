using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.Enums;
using UniSystem.Domain.Interfaces;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Statement : Entity<StatementId>
{
    public StudentId StudentId { get; private set; }
    public SecretaryId SecretaryId { get; private set; }
    public DeanId DeanId { get; private set; }

    public IStatementModels StatementModel { get; private set; }
    public SecretaryReviewStatus SecretaryReview { get; private set; } = SecretaryReviewStatus.Unwatched;
    public bool IsSecretarySign { get; private set; } = false;
    public DeanReviewStatus DeanReview { get; private set; } = DeanReviewStatus.Unwatched;
    public bool IsDeanSign { get; private set; } = false;

    public void Reject(SecretaryId secretaryId)
    {
        if (SecretaryId == secretaryId)
            SecretaryReview = SecretaryReviewStatus.Rejected;
    }
    
    public void Approve(SecretaryId secretaryId)
    {
        if (SecretaryId == secretaryId)
            SecretaryReview = SecretaryReviewStatus.Approved;
    }

    public void SignUp(SecretaryId secretaryId)
    {
        if (SecretaryId != secretaryId)
            throw new ArgumentException();
        
        if(!(SecretaryReview is SecretaryReviewStatus.Rejected or SecretaryReviewStatus.Approved))
            throw new ArgumentException();
            
        IsSecretarySign = true;
    }

    public void SignUp(DeanId deanId)
    {
        if (DeanId != deanId) 
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