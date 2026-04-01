using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Teacher: Member<TeacherId>
{
    public List<SpecialityId> SpecialitiesId { get; private set; } = new();
    public List<UniversityGroupId> TeachingGroupsId { get; private set; } = new();

    public List<UniversitySubjectId> TeachingSubjectsId { get; private set; } = new();

    public void AddUniversityGroup(UniversityGroupId groupId)
    {
        if(TeachingGroupsId.Contains(groupId))
            throw new Exception();
        
        TeachingGroupsId.Add(groupId);
    }

    public void RemoveUniversityGroup(UniversityGroupId groupId)
    {
        if(!TeachingGroupsId.Contains(groupId))
            throw new Exception();
        
        TeachingGroupsId.Remove(groupId);
    }
}