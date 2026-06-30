using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentStatus;

namespace UniSystem.Application.Documents.Commands.SecretaryReview;

public enum SecretaryDecision
{
    ApproveToDean,
    ReturnToRework,
    Reject
}

public record SecretaryReviewCommand(Guid DocumentId, SecretaryDecision Decision, string? ResolutionComment = null) : IRequest;

public class SecretaryReviewCommandHandler : IRequestHandler<SecretaryReviewCommand>
{
    private readonly IApplicationDbContext _context;

    public SecretaryReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SecretaryReviewCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .Include(d => d.DocumentType)
            .FirstOrDefaultAsync(d => d.Id == new DocumentId(request.DocumentId), cancellationToken);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.CurrentStatus.Name.Value != "На проверке секретаря")
            throw new DomainException("Document is not under secretary review.");

        _context.Entry(document).Property("SecretaryCheckAt").CurrentValue = DateTimeOffset.UtcNow;

        switch (request.Decision)
        {
            case SecretaryDecision.ApproveToDean:
            {
                var deanStatus = await _context.DocumentStatuses
                    .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("На проверке декана"), cancellationToken);

                if (deanStatus is null)
                    throw new DomainException("Status 'На проверке декана' not found.");

                _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = deanStatus.Id;
                _context.Entry(document).Property("DocumentSecretaryStatusId").CurrentValue = deanStatus.Id;
                break;
            }

            case SecretaryDecision.ReturnToRework:
            {
                var reworkStatus = await _context.DocumentStatuses
                    .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("На доработку"), cancellationToken);

                if (reworkStatus is null)
                    throw new DomainException("Status 'На доработку' not found.");

                _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = reworkStatus.Id;
                _context.Entry(document).Property("DocumentSecretaryStatusId").CurrentValue = reworkStatus.Id;

                if (request.ResolutionComment is not null)
                    _context.Entry(document).Property("ResolutionComment").CurrentValue = request.ResolutionComment;

                break;
            }

            case SecretaryDecision.Reject:
            {
                var rejectedStatus = await _context.DocumentStatuses
                    .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("Отклонён"), cancellationToken);

                if (rejectedStatus is null)
                    throw new DomainException("Status 'Отклонён' not found.");

                _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = rejectedStatus.Id;
                _context.Entry(document).Property("DocumentSecretaryStatusId").CurrentValue = rejectedStatus.Id;

                if (request.ResolutionComment is not null)
                    _context.Entry(document).Property("ResolutionComment").CurrentValue = request.ResolutionComment;

                break;
            }

            default:
                throw new DomainException("Invalid secretary decision.");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
