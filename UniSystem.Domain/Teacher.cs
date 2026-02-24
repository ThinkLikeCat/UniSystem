namespace UniSystem.Domain;

public class Teacher: Member
{
    public List<UniversityGroup> UniversityGroups { get; private set; }

    public void AddUniversityGroup(UniversityGroup group) => UniversityGroups.Add(group);
    
    public void RemoveUniversityGroup(UniversityGroup group) => UniversityGroups.Remove(group);
}