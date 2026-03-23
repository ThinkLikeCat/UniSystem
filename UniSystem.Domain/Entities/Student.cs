using UniSystem.Domain.Entities.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class Student: Member<StudentId>
{
    public Speciality Speciality { get; private set; }
    public int Course { get; private set; }
    public UniversityGroup UniversityGroup { get; private set; }

    public string State { get; private set; } = string.Empty;
    public bool IsCompletedEducation { get; private set; }

    public void NextYearTransfer()
    {
        if (IsCompletedEducation)
            throw new ArgumentOutOfRangeException($"Student {FullName} has completed education");
        
        Course++;
        
        if (Course > Speciality.MaxCourse)
            IsCompletedEducation = true;
    }

    public Student() { Id = StudentId.NewId(); }

    public Student(string fullName, DateTime birthDate, string password, string email, Speciality speciality,
        UniversityGroup universityGroup) :
        base(fullName, birthDate, email, password)
    {
        Id = StudentId.NewId();
        Speciality = speciality;
        UniversityGroup = universityGroup;
    }
}