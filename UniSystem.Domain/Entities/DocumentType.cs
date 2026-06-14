using UniSystem.Domain.ValueObjects.DocumentType;

namespace UniSystem.Domain.Entities;

public class DocumentType
{
    public int Id { get; private set; }

    public DocumentTypeName Name { get; private set; } = null!;
    public bool RequiresAttachments { get; private set; } = false;
    public TemplateText TemplateText { get; private set; } = null!;
    
    protected DocumentType() { }
    
    public DocumentType(string name, bool requiresAttachments, string templateText)
    {
        Name = new (name);
        RequiresAttachments = requiresAttachments;
        TemplateText = new (templateText);
    }
}