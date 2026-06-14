using UniSystem.Domain.Common;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StudentProfile : Entity<StudentId>
{
    public StudentProfile(StudentId id) : base(id) { }
    protected StudentProfile() { }

    public string StudentTicket { get; private set; } = string.Empty;

    public int AcademicGroupId { get; private set; }
    public AcademicGroup AcademicGroup { get; set; } = null!;

    public int StudentStatusId { get; set; }
    public StudentStatus StudentStatus { get; set; } = null!;

    public User User { get; set; } = null!;
}