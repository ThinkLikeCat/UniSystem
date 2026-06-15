using UniSystem.Domain.ValueObjects.Specialty;

namespace UniSystem.Domain.Entities;

public class StudentStatus
{
    public int Id { get; private set; }

    public SpecialtyName Name { get; private set; } = null!;

    protected StudentStatus() { }
    
    public StudentStatus(string name)
    {
        Name = new(name);
    }
}