using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class Teacher: Member<TeacherId>
{
    public List<Speciality> Specialities { get; private set; } = new();
    
    public List<UniversityGroup> TeachingGroups { get; private set; } = new();
    public List<UniversitySubject> TeachingSubjects { get; private set; } = new();
    
    public Teacher() {}
    
    public Teacher(string fullName, DateTime birthDate, string email, string password, List<Speciality> specialities,
        List<UniversityGroup> groups, List<UniversitySubject> subjects):
        base(fullName, birthDate, email, password)
    {
        Id = TeacherId.NewId();
        Specialities = specialities;
        TeachingGroups = groups;
        TeachingSubjects = subjects;
    }
    
    public void AddUniversityGroup(UniversityGroup group) => TeachingGroups.Add(group);
    
    public void RemoveUniversityGroup(UniversityGroup group) => TeachingGroups.Remove(group);
}