using UniSystem.Domain.ValueObjects.StudentStatus;

namespace UniSystem.Domain.Entities;

public class StudentStatus
{
    public int Id { get; private set; }

    public StatusName Name { get; private set; } = null!;

    protected StudentStatus() { }
    
    public StudentStatus(string name)
    {
        Name = new(name);
    }
}