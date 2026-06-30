using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Application.Documents.Commands;

public record UploadAttachmentCommand(Guid DocumentId, string FileName, Stream Content, long FileSize) : IRequest<Guid>;

public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IUserContext _userContext;

    public UploadAttachmentCommandHandler(
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        IUserContext userContext)
    {
        _context = context;
        _fileStorage = fileStorage;
        _userContext = userContext;
    }

    public async Task<Guid> Handle(UploadAttachmentCommand request, CancellationToken ct)
    {
        var userId = _userContext.UserId ?? throw new DomainException("User is not authenticated.");

        var document = await _context.Documents
            .Include(d => d.CurrentStatus)
            .FirstOrDefaultAsync(d => d.Id == new DocumentId(request.DocumentId), ct);

        if (document is null)
            throw new DomainException("Document not found.");

        if (document.AuthorId != userId)
            throw new DomainException("You are not the author of this document.");

        var statusName = document.CurrentStatus.Name.Value;
        if (statusName != "Черновик" && statusName != "На доработку")
            throw new DomainException("Files can only be attached to a draft.");

        var attachmentId = Guid.NewGuid();
        var filePath = await _fileStorage.SaveAsync(request.DocumentId, request.FileName, request.Content, ct);

        var attachment = new DocumentAttachment(
            new AttachmentId(attachmentId),
            new DocumentId(request.DocumentId),
            filePath,
            request.FileName,
            request.FileSize);

        _context.DocumentAttachments.Add(attachment);
        await _context.SaveChangesAsync(ct);

        return attachmentId;
    }
}
