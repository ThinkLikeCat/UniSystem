namespace UniSystem.Domain;

public class Speciality
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public int MaxCourse{ get; private set; }
    
    public List<UniversitySubject> UniversitySubjects { get; private set; }
}