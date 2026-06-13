namespace UniSystem.Domain.Entities;

public class DocumentType
{
    public int Id { get; set; }

    public string Name { get; private set; } = string.Empty;
    public bool RequiresAttachments { get; private set; } = false;
    public string TemplateText { get; private set; } = string.Empty;
    
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Имя не может превышать 100 символов.", nameof(name));

        Name = name;
    }
    
    public void SetRequiresAttachments(bool requiresAttachments) =>
        RequiresAttachments = requiresAttachments;
    
    public void SetTemplateText(string templateText)
    {
        if (string.IsNullOrWhiteSpace(templateText))
            throw new ArgumentException("Текст шаблона не может быть пустым.", nameof(templateText));

        TemplateText = templateText;
    }
}