namespace UniSystem.Domain.Entities;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя не может быть пустым или состоять только из пробелов.", nameof(name));

        if (name.Length > 150)
            throw new ArgumentException("Имя не может превышать 150 символов.", nameof(name));

        Name = name.Trim();
    }
}