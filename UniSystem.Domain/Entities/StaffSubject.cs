using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.Entities;

public class StaffSubject
{
    public Guid StaffId { get; private set; }
    public StaffProfile StaffProfile { get; private set; } = null!;

    public int SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;

    protected StaffSubject() { }

    public StaffSubject(Guid staffId, int subjectId)
    {
        if (subjectId <= 0)
            throw new InvalidStaffSubjectReferenceException(subjectId);
        
        StaffId = staffId;
        SubjectId = subjectId;
    }
}