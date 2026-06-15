using UniSystem.Domain.ValueObjects.DocumentStatus;

namespace UniSystem.Domain.Entities;

public class DocumentStatus
{
    public int Id { get; private set; }

    public DocumentStatusName Name { get; private set; } = null!;
    
    protected DocumentStatus() { }

    public DocumentStatus(DocumentStatusName name)
    {
        Name = new (name);
    }
}