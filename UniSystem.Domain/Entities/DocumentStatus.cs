namespace UniSystem.Domain.Entities;

public class DocumentStatus
{
    public int Id { get; set; }

    public string Name { get; private set; } = string.Empty;

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(name));
        
        if(name.Length > 50)
            throw new ArgumentException("Имя не может быть длиннее 50 символов.", nameof(name));
        
        Name = name;
    }
}