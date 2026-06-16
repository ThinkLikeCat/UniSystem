using UniSystem.Domain.Common;
using UniSystem.Domain.Exceptions;
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
            throw new InvalidStudentGroupReferenceException(academicGroupId);
        
        if (studentStatusId <= 0)
            throw new InvalidStudentStatusReferenceException(studentStatusId);
        
        StudentTicket = studentTicket;
        AcademicGroupId = academicGroupId;
        StudentStatusId = studentStatusId;
    }
    
}