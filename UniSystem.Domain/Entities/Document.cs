using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;
using UniSystem.Domain.ValueObjects.DocumentType;

namespace UniSystem.Domain.Entities;

public class Document : Entity<DocumentId>
{
    public Document(DocumentId id) : base(id) { }
    protected Document() { }

    public StudentId StudentId { get; set; }
    public StudentProfile Student { get; set; } = null!;

    public int DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; } = null!;
    
    public int DocumentCurrentStatusId { get; set; }
    public DocumentStatus CurrentStatus { get; set; } = null!;

    public int? DocumentSecretaryStatusId { get; set; }
    public DocumentStatus? SecretaryStatus { get; set; } = null!;

    public int? DocumentDeanStatusId { get; set; }
    public DocumentStatus? DeanStatus { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SendToReviewAt { get; set; } = null;
    public DateTimeOffset? SecretaryCheckAt { get; set; } = null;
    public DateTimeOffset? DeanCheckAt { get; set; } = null;

    public string DynamicValues { get; set; } = "{}";
    public string? ResolutionComment { get; set; } = null;

    public UserId? ResolvedByUserId { get; set; }
    public User? ResolvedByUser { get; set; }

    public ICollection<DocumentAttachment> Attachments { get; set; } = new List<DocumentAttachment>();
    
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