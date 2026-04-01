namespace UniSystem.Domain.Entities;

public class Curator: Teacher
{
    public UniversityGroup SupervisesGroup { get; private set; }
}