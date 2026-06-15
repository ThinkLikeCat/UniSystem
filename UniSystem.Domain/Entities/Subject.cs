using UniSystem.Domain.ValueObjects.Subject;

namespace UniSystem.Domain.Entities;

public class Subject
{
    public int Id { get; private set; }

    public SubjectName Name { get; private set; } = null!;
    
    protected Subject() { }

    public Subject(string name)
    {
        Name = new(name);
    }
}