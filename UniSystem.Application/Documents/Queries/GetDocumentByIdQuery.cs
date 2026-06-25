using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.Documents.Queries;

public record DocumentDetailDto(
    Guid Id,
    int DocumentTypeId,
    string AuthorName,
    string DocumentTypeName,
    string CurrentStatusName,
    string? SecretaryStatusName,
    string? DeanStatusName,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SendToReviewAt,
    DateTimeOffset? SecretaryCheckAt,
    DateTimeOffset? DeanCheckAt,
    string? DynamicValues,
    string? ResolutionComment,
    string? ResolvedByUserName,
    List<AttachmentDto> Attachments
);

public record AttachmentDto(Guid Id, string FileName, long FileSize, DateTimeOffset UploadedAt);

public record GetDocumentByIdQuery(Guid Id) : IRequest<DocumentDetailDto>;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentDetailDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _context.Documents
            .Include(d => d.Author)
            .Include(d => d.DocumentType)
            .Include(d => d.CurrentStatus)
            .Include(d => d.SecretaryStatus)
            .Include(d => d.DeanStatus)
            .Include(d => d.ResolvedByUser)
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id.Value == request.Id, cancellationToken);

        if (document is null)
            throw new DomainException("Document not found.");

        return new DocumentDetailDto(
            document.Id.Value,
            document.DocumentType.Id,
            document.Author.FullName,
            document.DocumentType.Name.Value,
            document.CurrentStatus.Name.Value,
            document.SecretaryStatus?.Name.Value,
            document.DeanStatus?.Name.Value,
            document.CreatedAt,
            document.SendToReviewAt,
            document.SecretaryCheckAt,
            document.DeanCheckAt,
            document.DynamicValues,
            document.ResolutionComment,
            document.ResolvedByUser?.FullName,
            document.Attachments.Select(a => new AttachmentDto(
                a.Id.Value,
                a.OriginalFileName.Value,
                a.FileSize.Value,
                a.UploadedAt
            )).ToList()
        );
    }
}
