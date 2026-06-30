using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Application.Documents.Queries;

public record DownloadAttachmentDto(Stream Content, string FileName, string ContentType);

public record DownloadAttachmentQuery(Guid AttachmentId) : IRequest<DownloadAttachmentDto>;

public class DownloadAttachmentQueryHandler : IRequestHandler<DownloadAttachmentQuery, DownloadAttachmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public DownloadAttachmentQueryHandler(IApplicationDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<DownloadAttachmentDto> Handle(DownloadAttachmentQuery request, CancellationToken ct)
    {
        var attachment = await _context.DocumentAttachments
            .FirstOrDefaultAsync(a => a.Id == new AttachmentId(request.AttachmentId), ct);

        if (attachment is null)
            throw new DomainException("File not found.");

        var stream = await _fileStorage.GetAsync(attachment.FilePath.Value, ct);
        if (stream is null)
            throw new DomainException("File not found on disk.");

        var contentType = GetContentType(attachment.OriginalFileName.Value);
        return new DownloadAttachmentDto(stream, attachment.OriginalFileName.Value, contentType);
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }
}
