using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Documents.Commands;

public record DeleteAttachmentCommand(Guid AttachmentId) : IRequest;

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IUserContext _userContext;

    public DeleteAttachmentCommandHandler(
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        IUserContext userContext)
    {
        _context = context;
        _fileStorage = fileStorage;
        _userContext = userContext;
    }

    public async Task Handle(DeleteAttachmentCommand request, CancellationToken ct)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var attachment = await _context.DocumentAttachments
            .Include(a => a.Document)
                .ThenInclude(d => d.CurrentStatus)
            .FirstOrDefaultAsync(a => a.Id.Value == request.AttachmentId, ct);

        if (attachment is null)
            throw new DomainException("File not found.");

        if (attachment.Document.AuthorId != userId)
            throw new DomainException("You are not the author of this document.");

        var statusName = attachment.Document.CurrentStatus.Name.Value;
        if (statusName != "Черновик" && statusName != "На доработку")
            throw new DomainException("Files can only be deleted from a draft.");

        await _fileStorage.DeleteAsync(attachment.FilePath.Value, ct);
        _context.DocumentAttachments.Remove(attachment);
        await _context.SaveChangesAsync(ct);
    }
}
