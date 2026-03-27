using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects.Id;

namespace UniSystem.Domain.Entities;

public class Student: Member<StudentId>
{
    public Speciality? Speciality { get; private set; }
    public int Course { get; private set; }
    public UniversityGroup? UniversityGroup { get; private set; }

    public string State { get; private set; } = string.Empty;
    public bool IsCompletedEducation { get; private set; }

    public Student() {}

    public Student(string fullName, DateTime birthDate, string password, string email, Speciality speciality,
        UniversityGroup universityGroup) :
        base(fullName, birthDate, email, password)
    {
        Speciality = speciality;
        UniversityGroup = universityGroup;
    }
    
    public void NextYearTransfer()
    {
        if (Speciality is null)
            throw new NullReferenceException("For this operation student should have speciality");
        
        if (IsCompletedEducation)
            throw new ArgumentOutOfRangeException($"Student {FullName} has completed education");
        
        Course++;
        
        if (Course > Speciality.MaxCourse)
            IsCompletedEducation = true;
    }

    public void AddToUniversityGroup(UniversityGroup universityGroup)
    {
        if (UniversityGroup is not null || Speciality is not null)
            return;
        
        universityGroup.AddStudent(this);
    }

    public void TransferToAnotherUniversityGroup(UniversityGroup newUniversityGroup)
    {
        if(newUniversityGroup is null)
            throw new ArgumentNullException("New group can't be null");
        
        if (UniversityGroup == newUniversityGroup)
            return;
        
        Speciality = newUniversityGroup.Speciality;
        UniversityGroup = newUniversityGroup;
    }
}