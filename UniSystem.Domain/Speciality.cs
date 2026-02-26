namespace UniSystem.Domain;

public class Speciality
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int MaxCourse{ get; private set; }
    
    public List<UniversitySubject> UniversitySubjects { get; private set; }

    public Speciality(string name, string description, int maxCourse, List<UniversitySubject> universitySubjects)
    {
        Name = name;
        Description = description;
        MaxCourse = maxCourse;
        UniversitySubjects = universitySubjects;
    }
}