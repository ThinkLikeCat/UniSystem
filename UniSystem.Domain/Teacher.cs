namespace UniSystem.Domain;

public class Teacher: Member
{
    public List<UniversityGroup> TeachingGroups { get; private set; }
    public List<UniversitySubject> TeachingSubjects { get; private set; }

    public void AddUniversityGroup(UniversityGroup group) => TeachingGroups.Add(group);
    
    public void RemoveUniversityGroup(UniversityGroup group) => TeachingGroups.Remove(group);
}