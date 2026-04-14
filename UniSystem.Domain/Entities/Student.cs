using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Student: Member<StudentId>
{
    public Student(string fullName, DateTime birthDate, string email, string password, SpecialityId? specialityId, int course, UniversityGroupId? universityGroupId, bool isCompletedEducation) : base(fullName, birthDate, email, password)
    {
        SpecialityId = specialityId;
        UniversityGroupId = universityGroupId;
        IsCompletedEducation = isCompletedEducation;
    }

    public SpecialityId? SpecialityId { get; private set; }
    public UniversityGroupId? UniversityGroupId { get; private set; }

    public string State { get; private set; } = string.Empty; //Need reworking
    public bool IsCompletedEducation { get; private set; }
    
    /*public void NextYearTransfer(Speciality speciality)
    {
        if (SpecialityId is null)
            throw new Exception("For this operation student should have speciality");
        
        if(SpecialityId != speciality.Id)
            throw new Exception($"Speciality {speciality.Id} has not been specified");
        
        if (IsCompletedEducation)
            throw new Exception($"Student {FullName} has completed education");
        
        Course++;
        
        if (Course > speciality.MaxCourse)
            IsCompletedEducation = true;
    }*/

    public void SetGroupAndSpeciality(UniversityGroupId newUniversityGroupId, SpecialityId newSpecialityId)
    {
        UniversityGroupId = newUniversityGroupId;
        SpecialityId = newSpecialityId;
    }
}