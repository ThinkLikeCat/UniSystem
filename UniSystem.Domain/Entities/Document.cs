using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentType;

namespace UniSystem.Domain.Entities;

public class Document : Entity<DocumentId>
{
    public StudentId StudentId { get; private set; }
    public StudentProfile Student { get; private set; } = null!;

    public int DocumentTypeId { get; private set; }
    public DocumentType DocumentType { get; private set; } = null!;
    
    public int DocumentCurrentStatusId { get; private set; }
    public DocumentStatus CurrentStatus { get; private set; } = null!;

    public int? DocumentSecretaryStatusId { get; private set; }
    public DocumentStatus? SecretaryStatus { get; private set; } = null!;

    public int? DocumentDeanStatusId { get; private set; }
    public DocumentStatus? DeanStatus { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? SendToReviewAt { get; private set; } = null;
    public DateTimeOffset? SecretaryCheckAt { get; private set; } = null;
    public DateTimeOffset? DeanCheckAt { get; private set; } = null;

    public string DynamicValues { get; private set; } = "{}";
    public string? ResolutionComment { get; private set; } = null;

    public UserId? ResolvedByUserId { get; private set; }
    public User? ResolvedByUser { get; private set; }

    public ICollection<DocumentAttachment> Attachments { get; set; } = new List<DocumentAttachment>();
    
    protected Document() { }
    public Document(DocumentId id, StudentId studentId, int documentTypeId, int initialStatusId) : base(id)
    {
        if (documentTypeId <= 0)
            throw new ArgumentException("Невалидный ID типа документа.", nameof(documentTypeId));
            
        if (initialStatusId <= 0)
            throw new ArgumentException("Невалидный ID начального статуса.", nameof(initialStatusId));

        StudentId = studentId;
        DocumentTypeId = documentTypeId;
        DocumentCurrentStatusId = initialStatusId;
        
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public IReadOnlyList<TemplateToken> GetTemplateTokens()
    {
        if (DocumentType is null)
            throw new InvalidOperationException("Тип документа не загружен.");
            
        if (Student is null)
            throw new InvalidOperationException("Данные студента не загружены.");
        
        var systemValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "{student_name}", Student.User.FullName},
            { "{group_name}",   Student.AcademicGroup?.Name ?? string.Empty },
            { "{course}",       Student.AcademicGroup?.Course.ToString() ?? string.Empty },
            { "{specialty}",    Student.AcademicGroup?.Specialty.Name ?? string.Empty }
        };
        
        return DocumentType.TemplateText.Tokenize(systemValues);
    }
}