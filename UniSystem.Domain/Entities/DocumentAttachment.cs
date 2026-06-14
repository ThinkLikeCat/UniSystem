using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentAttachment;

namespace UniSystem.Domain.Entities;

public class DocumentAttachment: Entity<AttachmentId>
{
    public DocumentId DocumentId { get; private set; }
    public Document Document { get; private set; }

    public FilePath FilePath { get; private set; } = null!;
    public FileName OriginalFileName { get; private set; } = null!;
    public FileSize FileSize { get; private set; } = null!;
    public DateTimeOffset UploadedAt { get; private set; }
    
    protected DocumentAttachment() { }
    
    public DocumentAttachment(AttachmentId id, DocumentId documentId, string filePath, string originalFileName, long fileSize)
        : base(id)
    {
        DocumentId = documentId;
        FilePath = new FilePath(filePath);
        OriginalFileName = new FileName(originalFileName);
        FileSize = new FileSize(fileSize);
        UploadedAt = DateTimeOffset.UtcNow;
    }
}