using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.DocumentStatus;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Application.Documents.Commands;

public record CreateDocumentCommand(
    int DocumentTypeId,
    string? DynamicValues = null
) : IRequest<Guid>;

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public CreateDocumentCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var documentType = await _context.DocumentTypes
            .FirstOrDefaultAsync(t => t.Id == request.DocumentTypeId, cancellationToken);

        if (documentType is null)
            throw new DomainException("Document type not found.");

        var draftStatus = await _context.DocumentStatuses
            .FirstOrDefaultAsync(s => s.Name == new DocumentStatusName("Черновик"), cancellationToken);

        if (draftStatus is null)
            throw new DomainException("Status 'Черновик' not found. Run SeedData.");

        var document = new Document(new DocumentId(Guid.NewGuid()), userId, request.DocumentTypeId, draftStatus.Id);

        if (!string.IsNullOrWhiteSpace(request.DynamicValues))
            _context.Entry(document).Property("DynamicValues").CurrentValue = request.DynamicValues;

        _context.Documents.Add(document);
        await _context.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
