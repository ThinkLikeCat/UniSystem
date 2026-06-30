using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentStatus;

namespace UniSystem.Application.Documents.Commands.DeanReview;

public enum DeanDecision
{
    Approve,
    Reject
}

public record DeanReviewCommand(Guid DocumentId, DeanDecision Decision, string? ResolutionComment = null) : IRequest;

public class DeanReviewCommandHandler : IRequestHandler<DeanReviewCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public DeanReviewCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(DeanReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .FirstOrDefaultAsync(d => d.Id == new DocumentId(request.DocumentId), cancellationToken);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.CurrentStatus.Name.Value != "На проверке декана")
            throw new DomainException("Document is not under dean review.");

        _context.Entry(document).Property("DeanCheckAt").CurrentValue = DateTimeOffset.UtcNow;
        _context.Entry(document).Property("ResolvedByUserId").CurrentValue = userId;

        switch (request.Decision)
        {
            case DeanDecision.Approve:
            {
                var approvedStatus = await _context.DocumentStatuses
                    .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("Утверждён"), cancellationToken);

                if (approvedStatus is null)
                    throw new DomainException("Status 'Утверждён' not found.");

                _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = approvedStatus.Id;
                _context.Entry(document).Property("DocumentDeanStatusId").CurrentValue = approvedStatus.Id;
                break;
            }

            case DeanDecision.Reject:
            {
                var rejectedStatus = await _context.DocumentStatuses
                    .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("Отклонён"), cancellationToken);

                if (rejectedStatus is null)
                    throw new DomainException("Status 'Отклонён' not found.");

                _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = rejectedStatus.Id;
                _context.Entry(document).Property("DocumentDeanStatusId").CurrentValue = rejectedStatus.Id;
                break;
            }

            default:
                throw new DomainException("Invalid dean decision.");
        }

        if (request.ResolutionComment is not null)
            _context.Entry(document).Property("ResolutionComment").CurrentValue = request.ResolutionComment;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
