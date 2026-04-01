using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Student: Member<StudentId>
{
    public SpecialityId? SpecialityId { get; private set; }
    public int Course { get; private set; }
    public UniversityGroupId? UniversityGroupId { get; private set; }

    public string State { get; private set; } = string.Empty; //Need reworking
    public bool IsCompletedEducation { get; private set; }
    
    public void NextYearTransfer(Speciality speciality)
    {
        if (SpecialityId is null)
            throw new NullReferenceException("For this operation student should have speciality");
        
        if(SpecialityId != speciality.Id)
            throw new ArgumentOutOfRangeException($"Speciality {speciality.Id} has not been specified");
        
        if (IsCompletedEducation)
            throw new ArgumentOutOfRangeException($"Student {FullName} has completed education");
        
        Course++;
        
        if (Course > speciality.MaxCourse)
            IsCompletedEducation = true;
    }

    public void SetGroupAndSpeciality(UniversityGroupId newUniversityGroupId, SpecialityId newSpecialityId)
    {
        UniversityGroupId = newUniversityGroupId;
        SpecialityId = newSpecialityId;
    }
}