using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects;

namespace UniSystem.Domain.Entities;

public class StaffSubject
{
    public UserId StaffId { get; private set; }
    public StaffProfile StaffProfile { get; private set; } = null!;

    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;

    protected StaffSubject() { }

    public StaffSubject(UserId staffId, int subjectId)
    {
        if (subjectId <= 0)
            throw new InvalidStaffSubjectReferenceException(subjectId);
        
        StaffId = staffId;
        SubjectId = subjectId;
    }
}