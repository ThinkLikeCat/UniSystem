using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StudentProfile : Entity<StudentId>
{
    public string StudentTicket { get; private set; } = string.Empty;

    public int AcademicGroupId { get; private set; }
    public AcademicGroup AcademicGroup { get; private set; } = null!;

    public int StudentStatusId { get; private set; }
    public StudentStatus StudentStatus { get; private set; } = null!;

    public User User { get; private set; } = null!;
    
    protected StudentProfile() { }

    public StudentProfile(StudentId id, string studentTicket, int academicGroupId, int studentStatusId) : base(id)
    {
        if (academicGroupId <= 0)
            throw new ArgumentException("Невалидный ID типа группы.", nameof(academicGroupId));
        
        if (studentStatusId <= 0)
            throw new ArgumentException("Невалидный ID статуса студента.", nameof(studentStatusId));
        
        StudentTicket = studentTicket;
        AcademicGroupId = academicGroupId;
        StudentStatusId = studentStatusId;
    }
    
}