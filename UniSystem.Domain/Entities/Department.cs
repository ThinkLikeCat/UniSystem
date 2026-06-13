namespace UniSystem.Domain.Entities;

public class Department
{
    public const int MaxNameLength = 150;
    
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    
    protected Department() { }
    
    public Department(string name)
    {
        ChangeName(name);
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Имя кафедры не может быть пустым.");

        var cleanedName = newName.Trim();

        if (cleanedName.Length > MaxNameLength)
            throw new ArgumentException($"Имя кафедры не может превышать {MaxNameLength} символов.");

        Name = cleanedName;
    }
}