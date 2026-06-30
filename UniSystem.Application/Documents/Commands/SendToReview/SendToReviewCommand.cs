using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentStatus;

namespace UniSystem.Application.Documents.Commands.SendToReview;

public record SendToReviewCommand(Guid DocumentId) : IRequest;

public class SendToReviewCommandHandler : IRequestHandler<SendToReviewCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public SendToReviewCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(SendToReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .Include(d => d.DocumentType)
            .FirstOrDefaultAsync(d => d.Id == new DocumentId(request.DocumentId), cancellationToken);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.AuthorId != userId)
            throw new DomainException("You are not the author of this document.");

        var statusName = document.CurrentStatus.Name.Value;

        if (statusName != "Черновик" && statusName != "На доработку")
            throw new DomainException("Document can only be sent from 'Черновик' or 'На доработку' status.");

        if (document.DocumentType.RequiresAttachments)
        {
            var attachmentCount = await _context.DocumentAttachments
                .CountAsync(a => a.DocumentId == new DocumentId(request.DocumentId), cancellationToken);

            if (attachmentCount == 0)
                throw new DomainException("This document type requires attached files.");
        }

        var secretaryStatus = await _context.DocumentStatuses
            .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("На проверке секретаря"), cancellationToken);

        if (secretaryStatus is null)
            throw new DomainException("Status 'На проверке секретаря' not found.");

        _context.Entry(document).Property("DocumentCurrentStatusId").CurrentValue = secretaryStatus.Id;
        _context.Entry(document).Property("SendToReviewAt").CurrentValue = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
