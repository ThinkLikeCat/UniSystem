using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Application.Documents.Commands;

public record DeleteDraftDocumentCommand(Guid DocumentId) : IRequest;

public class DeleteDraftDocumentCommandHandler : IRequestHandler<DeleteDraftDocumentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public DeleteDraftDocumentCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(DeleteDraftDocumentCommand request, CancellationToken ct)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .FirstOrDefaultAsync(d => d.Id == new DocumentId(request.DocumentId), ct);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.AuthorId != userId)
            throw new DomainException("You are not the author of this document.");

        if (document.CurrentStatus.Name.Value != "Черновик")
            throw new DomainException("Only drafts can be deleted.");

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync(ct);
    }
}
