using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Documents.Commands;

public record UpdateDocumentDynamicValuesCommand(Guid DocumentId, string DynamicValues) : IRequest;

public class UpdateDocumentDynamicValuesCommandHandler : IRequestHandler<UpdateDocumentDynamicValuesCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public UpdateDocumentDynamicValuesCommandHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task Handle(UpdateDocumentDynamicValuesCommand request, CancellationToken ct)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .FirstOrDefaultAsync(d => d.Id.Value == request.DocumentId, ct);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.AuthorId != userId)
            throw new DomainException("You are not the author of this document.");

        if (document.CurrentStatus.Name.Value != "Черновик" && document.CurrentStatus.Name.Value != "На доработку")
            throw new DomainException("DynamicValues can only be edited in 'Черновик' or 'На доработку' status.");

        _context.Entry(document).Property("DynamicValues").CurrentValue = request.DynamicValues;
        await _context.SaveChangesAsync(ct);
    }
}
