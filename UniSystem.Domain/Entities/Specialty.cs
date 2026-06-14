using UniSystem.Domain.ValueObjects.Specialty;

namespace UniSystem.Domain.Entities;

public class Specialty
{
    public int Id { get; private set; }

    public SpecialtyName Name { get; private set; } = null!;
    public SpecialtyCode Code { get; private set; } = null!;
    public SpecialtyDuration MaxDurationInYears { get; private set; } = null!;
    
    protected Specialty() { }

    public Specialty(string name, string code, short maxDurationInYears)
    {
        Name = new(name);
        Code = new(code);
        MaxDurationInYears = new(maxDurationInYears);
    }
}