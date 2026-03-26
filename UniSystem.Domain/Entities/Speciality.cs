using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class Speciality: Entity<SpecialityId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int MaxCourse { get; private set; } = 4;
    
    public List<UniversitySubject> UniversitySubjects { get; private set; }
    
    public Speciality() {}

    public Speciality(string name, string description, int maxCourse, List<UniversitySubject> universitySubjects)
    {
        Name = name;
        Description = description;
        MaxCourse = maxCourse;
        UniversitySubjects = universitySubjects;
    }
}