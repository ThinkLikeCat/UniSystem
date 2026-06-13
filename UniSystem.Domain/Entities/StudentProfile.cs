using UniSystem.Domain.Common;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StudentProfile : Entity<StudentId>
{
    public StudentProfile(StudentId id) : base(id) { }
    protected StudentProfile() { }

    public string StudentTicket { get; private set; } = string.Empty;
    public int Course { get; private set; }

    public int AcademicGroupId { get; private set; }
    public AcademicGroup AcademicGroup { get; set; } = null!;

    public int StudentStatusId { get; set; }
    public StudentStatus StudentStatus { get; set; } = null!;

    public User User { get; set; } = null!;
    
    public void ChangeCourse(int newCourse, Specialty specialty)
    {
        if (newCourse > specialty.MaxDurationInYears) 
        {
            throw new DomainException($"Нельзя установить курс {newCourse} для специальности {specialty.Name}. Максимум: {specialty.MaxDurationInYears}.");
        }

        if (newCourse < 1)
        {
            throw new DomainException("Курс не может быть меньше 1.");
        }

        Course = newCourse;
    }
}