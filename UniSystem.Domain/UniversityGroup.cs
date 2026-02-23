namespace UniSystem.Domain;

public class UniversityGroup
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    public Curator Curator { get; private set; }
    public List<Student> Students { get; private set; }
}