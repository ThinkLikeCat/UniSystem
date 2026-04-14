using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Curator: Teacher
{
    public UniversityGroupId SupervisesGroupId { get; private set; }
    
    public Curator(string fullName, DateTime birthDate, string email, string password) : base(fullName, birthDate, email, password)
    {
    }
}