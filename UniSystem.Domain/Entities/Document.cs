using UniSystem.Domain.Common;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentType;

namespace UniSystem.Domain.Entities;

public class Document : Entity<DocumentId>
{
    public Guid AuthorId { get; private set; }
    public User Author { get; private set; } = null!;

    public int DocumentTypeId { get; private set; }
    public DocumentType DocumentType { get; private set; } = null!;
    
    public int DocumentCurrentStatusId { get; private set; }
    public DocumentStatus CurrentStatus { get; private set; } = null!;

    public int? DocumentSecretaryStatusId { get; private set; } = null;
    public DocumentStatus? SecretaryStatus { get; private set; } = null;

    public int? DocumentDeanStatusId { get; private set; } = null;
    public DocumentStatus? DeanStatus { get; private set; } = null;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? SendToReviewAt { get; private set; } = null;
    public DateTimeOffset? SecretaryCheckAt { get; private set; } = null;
    public DateTimeOffset? DeanCheckAt { get; private set; } = null;

    public string? DynamicValues { get; private set; } = null;
    public string? ResolutionComment { get; private set; } = null;

    public Guid? ResolvedByUserId { get; private set; }
    public User? ResolvedByUser { get; private set; }

    public ICollection<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();
    
    protected Document() { }
    public Document(DocumentId id, Guid authorId, int documentTypeId, int initialStatusId) : base(id)
    {
        if (documentTypeId <= 0)
            throw new InvalidDocumentTypeReferenceException(documentTypeId);
            
        if (initialStatusId <= 0)
            throw new InvalidDocumentStatusReferenceException(initialStatusId);

        AuthorId = authorId;
        DocumentTypeId = documentTypeId;
        DocumentCurrentStatusId = initialStatusId;
        
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public static IReadOnlyList<TemplateToken> GetTemplateTokens(User author, DocumentType documentType)
    {
        if (documentType is null)
            throw new InvalidOperationException("Document type not loaded.");

        if (author is null)
            throw new InvalidOperationException("Document author not loaded.");

        var student = author.StudentProfile;

        var systemValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "{student_name}", author.FullName},
            { "{group_name}",   student?.AcademicGroup?.Name.Value ?? string.Empty },
            { "{course}",       student?.AcademicGroup?.Course.Value.ToString() ?? string.Empty },
            { "{specialty}",    student?.AcademicGroup?.Specialty.Name.Value ?? string.Empty }
        };

        return documentType.TemplateText.Tokenize(systemValues);
    }
}